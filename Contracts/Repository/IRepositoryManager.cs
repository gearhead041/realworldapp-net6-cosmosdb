
namespace Contracts.Repository;

public interface IRepositoryManager
{
    //Add Model Repos here
    //IObjectModelRepository ObjectModelRepository;
    IUserRepository UserRepository { get; }
    IArticleRepository ArticleRepository { get; }
    ICommentRepository CommentRepository { get; }
    Task Save();
}