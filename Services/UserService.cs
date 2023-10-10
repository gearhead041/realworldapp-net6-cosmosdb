using AutoMapper;
using Contracts.Repository;
using Contracts.Services;
using Entities.Dtos;
using Entities.Models;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Services;

internal class UserService : IUserService
{
    private readonly IRepositoryManager repositoryManager;
    private readonly IMapper mapper;

    public UserService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
    }

    private static bool Verify(string password, string userFromDbHash)
    {
        byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
        string loginHash = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(passwordBytes));
        bool verified = false;
        if (loginHash == userFromDbHash)
            verified = true;
        return verified;
    }


    public async Task<(bool, UserDto)> AuthenticateUser(UserForAuthDto userForAuth)
    {
        var user = await repositoryManager.UserRepository.GetUser(userForAuth.Email, false, null);
        if (user == null)
            return (false, null);
        var result = Verify(userForAuth.Password.Trim(), user.PasswordHash);
        var userWithToken = mapper.Map<UserDto>(user);
        userWithToken.Token = GenerateToken(userForAuth.Email);
        return (result, userWithToken);
    }

    private static string GenerateToken(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your-issuer",
            audience: "your-audience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // Set token expiration
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public async Task<(bool, UserDto)> CreateUser(CreateUserDto userCreate)
    {
        User userInDb;
        userInDb = await repositoryManager.UserRepository.GetUser(userCreate.Email, false, null);
        if (userInDb != null)
            return (false, null);
        userInDb = await repositoryManager.UserRepository.GetUserByName(userCreate.UserName, false, null);
        if (userInDb != null)
            return (false, null);
        var user = mapper.Map<User>(userCreate);
        byte[] passwordBytes = Encoding.ASCII.GetBytes(userCreate.Password.Trim());
        user.PasswordHash = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(passwordBytes));
        repositoryManager.UserRepository.CreateUser(user);
        await repositoryManager.Save();
        var userToReturn = mapper.Map<UserDto>(user);
        userToReturn.Token = GenerateToken(userCreate.Email);
        return (true, userToReturn);
    }

    public static string GetEmailFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = new JwtSecurityToken(jwtToken);
        // Find the claim by type
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        if (claim != null)
        {
            return claim.Value;
        }

        return null;
    }

    public async Task<UserDto> GetUser(string jwtToken)
    {
        var email = GetEmailFromToken(jwtToken);
        if (email == null)
            return null;

        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        if (user == null)
            return null;
        return mapper.Map<UserDto>(user);
    }

    public async Task<(bool, UserDto)> UpdateUser(UserDto userUpdate)
    {
        var user = await repositoryManager.UserRepository.GetUser(userUpdate.Email, true, null);
        if (user == null)
            return (false, null);
        var userCheck = await repositoryManager.UserRepository.GetUserByName(userUpdate.UserName, false, null);
        if (userCheck != null)
        {
            if (userCheck.Id != user.Id)
                return (false, null);
        }
        mapper.Map(userUpdate, user);
        await repositoryManager.Save();
        return (true, userUpdate);
    }

    public async Task<ProfileDto> FollowUser(string userName, string userToFollow)
    {
        var userFromDb = await repositoryManager.UserRepository.GetUserByName(userName, true, null);
        var userToFollowDb = await repositoryManager.UserRepository.GetUserByName(userToFollow, true, null);
        if (userFromDb == null || userToFollowDb == null)
            return null!;
        userFromDb.FollowingIds = userFromDb.FollowingIds
            .Append(userToFollowDb.Id.ToString()).ToArray();
        await repositoryManager.Save();
        var profile = mapper.Map<ProfileDto>(userToFollowDb);
        profile.Following = true;
        return profile;
    }

    public async Task<ProfileDto> GetProfile(string userName, string? token)
    {
        var user = await repositoryManager.UserRepository.GetUserByName(userName, false, null);
        if (user == null)
            return null;
        var profile = mapper.Map<ProfileDto>(user);
        if (token != null)
        {
            var email = GetEmailFromToken(token);
            var requestUser = await repositoryManager.UserRepository.GetUser(email,false,null);
            if(requestUser != null)
            {
                var following = requestUser.FollowingIds.FirstOrDefault( u => u == user.Id.ToString());
                if (following != null)
                    profile.Following = true;
            }
        }
        else
        {
            profile.Following = false;
        }

        return profile;
    }


    public async Task<ProfileDto> UnfollowUser(string userName, string userToUnFollow)
    {
        var userFromDb = await repositoryManager.UserRepository.GetUserByName(userName, true, null);
        var userToUnfollowDb = await repositoryManager.UserRepository.GetUserByName(userToUnFollow, true, null);
        if (userFromDb == null || userToUnfollowDb == null)
            return null!;
        userFromDb.FollowingIds = userFromDb.FollowingIds
            .Where(u => u != userToUnfollowDb.Id.ToString()).ToArray();
        await repositoryManager.Save();
        var profile = mapper.Map<ProfileDto>(userToUnfollowDb);
        profile.Following = false;
        return profile;
    }

}