
using Contracts.Repository;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext context;
        //private readonly Lazy<IObjectModelRepository> _objectModelRepository;
        public RepositoryManager(RepositoryContext context)
        {
            this.context = context;
            //_objectRepository = new Lazy<IObjectModelRepository>(()
            //=> new ObjectModelRepository(context));

        }
        //public IObejctModelRepository ObejectRepository => _objectModelRepository.Value;


        public async Task Save()
        {
            //uncomment line below if necessary
            //await context.Database.EnsureCreatedAsync();
            await context.SaveChangesAsync();
        }
    }
}