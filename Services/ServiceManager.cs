using AutoMapper;
using Contracts.Repository;
using Contracts.Services;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        //private readonly Lazy<IObjectModelService> objectModelService;
        private readonly Lazy<IUserService> _userService;
        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            //objectModelService = new Lazy<IObjectModelService>(()
            //=> new ObjectModelService(repositoryManager))
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager,mapper));
        }

        public IUserService UserService => _userService.Value;

        //IUserService IServiceManager.UserService => throw new NotImplementedException();
    }
}