using Entities.Dtos;

namespace Contracts.Services;

public interface IUserService
{
    Task<(bool, UserDto)> AuthenticateUser(UserForAuthDto userForAuth);
    Task<(bool,UserDto)> CreateUser(CreateUserDto userCreate);
    Task<UserDto> GetUser(string token);
    Task<UserDto> UpdateUser(UserDto user);
    Task<ProfileDto> GetProfile(string userName);
    Task<ProfileDto> FollowUser(string userName, string userToFollow);
    Task<ProfileDto> UnfollowUser(string userName, string userToFollow);
}