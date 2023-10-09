
using System.Linq.Expressions;

namespace Contracts.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll(bool trackChanges, string? include);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges, string? include);
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}