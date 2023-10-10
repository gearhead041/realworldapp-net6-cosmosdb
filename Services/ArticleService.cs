using AutoMapper;
using Contracts.Repository;
using Contracts.Services;
using Entities.Dtos;

namespace Services;

public class ArticleService : IArticleService
{
    private IRepositoryManager repositoryManager;
    private IMapper mapper;

    public ArticleService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
    }

    public Task<CommentDto> AddComment(string token, string slug, CommentDto commentToCreate)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto> CreateArticle(string token, CreateArticleDto createArticle)
    {
        throw new NotImplementedException();
    }

    public Task DeleteArticle(string slug)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto> DeleteComment(string slug, Guid commentId)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto> FavoriteArticle(string token, string slug)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto> GetArticle(string slug)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>> GetArticles(string? tag, string? author, string? favorited, int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>> GetUserFeed(string token)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto> UnfavoriteArticle(string token, string slug)
    {
        throw new NotImplementedException();
    }
}