using Entities.Models;

namespace Contracts.Repository;

public interface ICommentRepository
{
    Task<Comment> GetComment(Guid Id, bool trackChanges, string includes);
    void CreateComment(Comment comment);
    void DeleteComment(Comment comment);
}