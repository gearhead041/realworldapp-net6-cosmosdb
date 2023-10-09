using AutoMapper;
using Contracts.Repository;
using Contracts.Services;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        //private readonly Lazy<IObjectModelService> objectModelService;
        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            //objectModelService = new Lazy<IObjectModelService>(()
            //=> new ObjectModelService(repositoryManager))
        }
    }
}