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
using XAct;

namespace Services;

internal class UserService : IUserService
{
    private IRepositoryManager repositoryManager;
    private IMapper mapper;

    public UserService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
    }

    private bool Verify(string password, byte[] userFromDbHash)
    {
        byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
        byte[] loginHash = new MD5CryptoServiceProvider().ComputeHash(passwordBytes);
        bool verified = false;
        if (loginHash.Length == userFromDbHash.Length)
        {
            int i = 0;
            while ((i < loginHash.Length) && (loginHash[i] == userFromDbHash[i]))
            {
                i += 1;
            }
            if (i == loginHash.Length)
            {
                verified = true;
            }
        }
        return verified;
    }


    public async Task<(bool, UserDto)> AuthenticateUser(UserForAuthDto userForAuth)
    {
        var user = await repositoryManager.UserRepository.GetUser(userForAuth.Email, false, null);
        if (user == null)
            return (false, null);
        var result = Verify(userForAuth.Password, user.PasswordHash);
        var userWithToken = mapper.Map<UserDto>(user);
        userWithToken.Token = GenerateToken(userForAuth);
        return (result, mapper.Map<UserDto>(user));
    }

    private string GenerateToken(UserForAuthDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"));
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
        var userInDb = await repositoryManager.UserRepository.GetUser(userCreate.Email, false, null);
        if (userInDb != null)
            return (false, null);
        var user = mapper.Map<User>(userCreate);
        repositoryManager.UserRepository.CreateUser(user);
        await repositoryManager.Save();
        return (true, mapper.Map<UserDto>(user));
    }

    public static string GetEmailFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        // Find the claim by type
        var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        if (claim != null)
        {
            return claim.Value;
        }

        return null;
    }

    public async Task<UserDto> GetUser(string token)
    {
        var email = GetEmailFromToken(token);
        if (email.IsNull())
            return null;

        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        if (user == null)
            return null;
        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUser(UserDto userUpdate)
    {
        var user = await repositoryManager.UserRepository.GetUser(userUpdate.Email, true, null);
        if (user.IsNull())
            return null;
        mapper.Map(userUpdate, user);
        await repositoryManager.Save();
        return userUpdate;
    }

    public Task<ProfileDto> FollowUser(string userName, string userToFollow)
    {
        throw new NotImplementedException();
    }

    public Task<ProfileDto> GetProfile(string userName)
    {
        throw new NotImplementedException();
    }


    public Task<ProfileDto> UnfollowUser(string userName, string userToFollow)
    {
        throw new NotImplementedException();
    }

}