using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Interface
{
    public interface ICommentService
    {
        Task<CommentEntity> CreateCommentAsync(int taskId, int userId, string commentText);
        Task<CommentEntity> UpdateCommentAsync(int commentId, string text);
        Task<List<CommentEntity>> GetAllCommentsAsync();
        Task<CommentEntity> GetCommentByIdAsync(int commentId);
        Task<List<CommentEntity>> GetCommentsByTaskIdAsync(int taskId);
        Task<CommentServiceResponse> DeleteCommentAsync(int commentId);
    }
}
