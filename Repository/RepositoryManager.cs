
using Contracts.Repository;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext context;
        //private readonly Lazy<IObjectModelRepository> _objectModelRepository;
        private readonly Lazy<IUserRepository> userRepository;
        public RepositoryManager(RepositoryContext context)
        {
            this.context = context;
            //_objectRepository = new Lazy<IObjectModelRepository>(()
            //=> new ObjectModelRepository(context));

            userRepository = new Lazy<IUserRepository> (() =>
            new UserRepository(context);
        }
        //public IObejctModelRepository ObejectRepository => _objectModelRepository.Value;

        public IUserRepository UserRepository => userRepository.Value;
        public async Task Save()
        {
            //uncomment line below if necessary
            //await context.Database.EnsureCreatedAsync();
            await context.SaveChangesAsync();
        }
    }
}