using Entities.Dtos;

namespace Contracts.Services;

public interface IArticleService
{
    Task<IEnumerable<ArticleDto>> GetArticles(string? tag, string? author, 
        string? favorited, string? token, int limit, int offset);
    Task<IEnumerable<ArticleDto>> GetUserFeed(string token, int limit, int offset);
    Task<ArticleDto> GetArticle(string slug);
    Task<(bool,ArticleDto)> CreateArticle(string token, CreateArticleDto createArticle);
    Task<bool> DeleteArticle(string slug);
    Task<CommentDto> AddComment(string token, string slug, CommentCreateDto commentToCreate);
    Task<bool> DeleteComment(string slug, Guid commentId);
    Task<IEnumerable<CommentDto>> GetCommentsForArticle(string slug);
    Task<ArticleDto> FavoriteArticle(string token, string slug);
    Task<ArticleDto> UnfavoriteArticle(string token, string slug);
    Task<ArticleDto> UpdateArticle(string slug, UpdateArticleDto articleDto);

    Task<IEnumerable<string>> GetTags();
}