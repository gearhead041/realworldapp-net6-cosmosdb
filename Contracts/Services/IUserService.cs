using Entities.Dtos;

namespace Contracts.Services;

public interface IUserService
{
    Task<UserDto> AuthenticateUser(UserForAuthDto userForAuth);
    Task<UserDto> CreateUser(CreateUserDto userCreate);
    Task<UserDto> GetUser(string userId);
    Task<UserDto> UpdateUser(UserDto user);
    Task<ProfileDto> GetProfile(string userName);
    Task<ProfileDto> FollowUser(string userName, string userToFollow);
    Task<ProfileDto> UnfollowUser(string userName, string userToFollow);
}