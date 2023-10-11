using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    public CommentRepository(RepositoryContext context) : base(context)
    {
        
    }

    public void CreateComment(Comment comment)
        => Create(comment);

    public void DeleteComment(Comment comment)
        => Delete(comment);

    public async Task<IEnumerable<Comment>> GetAllComments(bool trackChanges, string? include)
        =>  await GetAll(trackChanges, include).ToListAsync();

    public async Task<Comment> GetComment(Guid Id, bool trackChanges, string inlcudes)
        => await FindByCondition(c => c.Id == Id, trackChanges, inlcudes).SingleOrDefaultAsync();
}   