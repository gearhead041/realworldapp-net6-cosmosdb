using Entities.Models;

namespace Contracts.Repository;

public interface ICommentRepository
{
    Task<Comment> GetComment(Guid Id, bool trackChanges, string includes);
    Task<IEnumerable<Comment>> GetAllComments(bool trackChanges, string include);
    void CreateComment(Comment comment);
    void DeleteComment(Comment comment);
}