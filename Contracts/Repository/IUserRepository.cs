using Entities.Dtos;
using Entities.Models;

namespace Contracts.Repository;

public interface IUserRepository
{
    Task<User> GetUser(string email, bool trackchanges, string? include);
    Task<User> GetUserByName(string name,bool trackchanges, string? include);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);
}