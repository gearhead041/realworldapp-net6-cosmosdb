using AutoMapper;
using Contracts.Repository;
using Contracts.Services;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        //private readonly Lazy<IObjectModelService> objectModelService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IArticleService> _articleService;
        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            //objectModelService = new Lazy<IObjectModelService>(()
            //=> new ObjectModelService(repositoryManager))
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager,mapper));
            _articleService = new Lazy<IArticleService> (() => new ArticleService(repositoryManager, mapper));
        }

        public IUserService UserService => _userService.Value;
        public IArticleService ArticleService => _articleService.Value;
        //IUserService IServiceManager.UserService => throw new NotImplementedException();
    }
}