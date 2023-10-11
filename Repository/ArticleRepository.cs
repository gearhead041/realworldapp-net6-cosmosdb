using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
{
    public ArticleRepository(RepositoryContext context) : base(context)
    {
    }

    public void CreateArticle(Article article)
        => Create(article);
    public void DeleteArticle(Article article)
        => Delete(article);

    public async Task<IEnumerable<Article>> GetAllArticles(bool trackChanges, string? includes)
        => await GetAll(trackChanges,includes).ToListAsync();

    public async Task<Article> GetArticle(string slug, bool trackChanges, string? inlcudes)
        => await FindByCondition( a => a.Slug == slug, trackChanges,inlcudes).FirstOrDefaultAsync();

    public void UpdateArticle(Article article)
        => Update(article);
}