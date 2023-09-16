using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Interface
{
    public interface ICommentService
    {
        Task<Comment> CreateCommentAsync(int taskId, int userId, string commentText);
        Task<Comment> UpdateCommentAsync(int commentId, string text);
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<List<Comment>> GetCommentsByTaskIdAsync(int taskId);
        Task<CommentServiceResponse> DeleteCommentAsync(int commentId);
    }
}
