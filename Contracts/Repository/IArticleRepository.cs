using Entities.Models;

namespace Contracts.Repository;

public interface IArticleRepository
{
    Task<Article> GetArticle(string slug, bool trackChanges, string? inlcudes);
    Task<IEnumerable<Article>> GetAllArticles(bool trackChanges, string? includes);
    void CreateArticle(Article article);
    void DeleteArticle(Article article);
    void UpdateArticle(Article article);
}