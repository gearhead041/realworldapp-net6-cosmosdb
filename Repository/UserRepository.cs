using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class UserRepository: RepositoryBase<User>, IUserRepository
{

    public UserRepository(RepositoryContext context) : base(context)
    {
    }

    public void CreateUser(User user)
        => Create(user);

    public void DeleteUser(User user)
       => Delete(user);

    public IQueryable<User> GetAll(bool trackChanges, string? include)
        => GetAll(trackChanges,include);

    public async Task<User> GetUser(string email, bool trackchanges, string? include)
        => await FindByCondition(u => u.Email == email, trackchanges, include).SingleOrDefaultAsync();

    public async Task<User> GetUserByName(string name, bool trackchanges, string? include)
        => await FindByCondition(u => u.UserName == name, trackchanges, null).SingleOrDefaultAsync();
    public void UpdateUser(User user)
        => Update(user);
}