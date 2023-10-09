using AutoMapper;
using Contracts.Repository;
using Contracts.Services;
using Entities.Dtos;
using Entities.Models;
using System.Text;
using XSystem.Security.Cryptography;

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


    public async Task<(bool,UserDto)> AuthenticateUser(UserForAuthDto userForAuth)
    {
        var user = await repositoryManager.UserRepository.GetUser(userForAuth.Email, false, null);
        if(user == null)
            return (false,null);
        var result = Verify(userForAuth.Password, user.PasswordHash);
        var userWithToken = mapper.Map<UserDto>(user);
        userWithToken.Token = GenerateToken();
        return (result, mapper.Map<UserDto>(user));
    }

    private string GenerateToken()
    {
        throw new NotImplementedException();
    }

    public async Task<(bool,UserDto)> CreateUser(CreateUserDto userCreate)
    {
        var userInDb = await repositoryManager.UserRepository.GetUser(userCreate.Email,false,null);
        if (userInDb != null)
            return (false, null);
        var user = mapper.Map<User>(userCreate);
        repositoryManager.UserRepository.CreateUser(user);
        await repositoryManager.Save();
        return (true, mapper.Map<UserDto>(user));
    }

    public async Task<UserDto> GetUser(string email)
    {
        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        if (user == null)
            return null;
        return mapper.Map<UserDto>(user);
    }

    public Task<UserDto> UpdateUser(UserDto user)
    {
        throw new NotImplementedException();
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