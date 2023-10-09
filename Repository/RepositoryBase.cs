using Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T> GetAll(bool trackChanges, string include)
        {

            IQueryable<T> query = !trackChanges ? RepositoryContext.Set<T>().AsNoTracking()
            : RepositoryContext.Set<T>();

            if (include != null)
            {
                var includes = include.Split(',');
                foreach (var item in includes)
                {
                    query.Include(item);
                }
            }
            return query;

        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
            bool trackChanges, string include)
        {
            IQueryable<T> query = !trackChanges ? RepositoryContext.Set<T>()
                            .Where(expression)
                            .AsNoTracking() :
                            RepositoryContext.Set<T>()
                            .Where(expression);
            if (include != null)
            {
                var includes = include.Split(',');
                foreach (var item in includes)
                {
                    query.Include(item);
                }
            }
            return query;
        }


        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

    }
}