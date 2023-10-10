using Entities.Dtos;

namespace Contracts.Services;

public interface IArticleService
{
    Task<IEnumerable<ArticleDto>> GetArticles(string? tag, string? author, string? favorited, int limit, int offset);
    Task<IEnumerable<ArticleDto>> GetUserFeed(string token);
    Task<ArticleDto> GetArticle(string slug);
    Task<ArticleDto> CreateArticle(string token, CreateArticleDto createArticle);
    Task DeleteArticle(string slug);
    Task<CommentDto> AddComment(string token, string slug, CommentDto commentToCreate);
    Task<CommentDto> DeleteComment(string slug, Guid commentId);
    Task<ArticleDto> FavoriteArticle(string token, string slug);
    Task<ArticleDto> UnfavoriteArticle(string token, string slug);
}