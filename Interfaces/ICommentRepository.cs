using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateCommentAsync(Comment comment);
}