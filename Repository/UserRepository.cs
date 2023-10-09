using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class UserRepository: RepositoryBase<User>, IUserRepository
{
    private RepositoryContext context;

    public UserRepository(RepositoryContext context) : base(context)
    {
        this.context = context;
    }

    public void CreateUser(User user)
        => Create(user);

    public void DeleteUser(User user)
       => Delete(user);

    public IQueryable<User> GetAll(bool trackChanges, string? include)
        => GetAll(trackChanges,include);

    public async Task<User> GetUser(string email, bool trackchanges, string? include)
        => await FindByCondition(u => u.UserName == username, trackchanges, include).SingleOrDefaultAsync();
;
    public void UpdateUser(User user)
        => Update(user);
}