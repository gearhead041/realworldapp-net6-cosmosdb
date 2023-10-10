
namespace Contracts.Services;

public interface IServiceManager
{
    //Model Service Interfaces Here
    IUserService UserService { get; }
    IArticleService ArticleService { get; }
}