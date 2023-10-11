using Entities.Dtos;

namespace Contracts.Services;

public interface IArticleService
{
    Task<IEnumerable<ArticleDto>> GetArticles(string? tag, string? author, 
        string? favorited, int limit, int offset, string? token);
    Task<IEnumerable<ArticleDto>> GetUserFeed(string token, int limit, int offset);
    Task<ArticleDto> GetArticle(string slug);
    Task<(bool,ArticleDto)> CreateArticle(string token, CreateArticleDto createArticle);
    Task<bool> DeleteArticle(string slug);
    Task<CommentDto> AddComment(string token, string slug, CommentDto commentToCreate);
    Task<bool> DeleteComment(string slug, Guid commentId);
    Task<ArticleDto> FavoriteArticle(string token, string slug);
    Task<ArticleDto> UnfavoriteArticle(string token, string slug);
    Task<IEnumerable<string>> GetTags();
}