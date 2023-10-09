using AutoMapper;
using Contracts.Repository;
using Contracts.Services;
using Entities.Dtos;

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

    public async Task<UserDto> AuthenticateUser(UserForAuthDto userForAuth)
    {
        var user = await repositoryManager.UserRepository.FInd
    }

    public Task<UserDto> CreateUser(CreateUserDto userCreate)
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

    public Task<UserDto> GetUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ProfileDto> UnfollowUser(string userName, string userToFollow)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUser(UserDto user)
    {
        throw new NotImplementedException();
    }
}