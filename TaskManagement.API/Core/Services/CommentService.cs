using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.DataAccess;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Hubs;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CommentHub> _hubContext;
        public CommentService(ApplicationDbContext context, IHubContext<CommentHub> hubContext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task<Comment> CreateCommentAsync(int taskId, int userId, string commentText)
        {
            var task = await _context.Tasks.FindAsync(taskId) ?? throw new NullReferenceException("taskId does not exist"); 
            var user = await _context.Users.FindAsync(userId) ?? throw new NullReferenceException("userId does not exist");
           
            var comment = new Comment
            {
                Text = commentText,
                Task = task,
                User = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Notify the task's creator (CreatedByUserId) when someone adds a comment to the task
            var ownerId = task.CreatedByUserId.ToString();
            await _hubContext.Clients.User(ownerId).SendAsync("ReceiveTaskCommentNotification", taskId, commentText);

            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(int commentId, string text)
        {
            var comment = await _context.Comments.FindAsync(commentId) ?? throw new NullReferenceException("comment does not exist"); 

            comment.Text = text; 

            await _context.SaveChangesAsync();

            return comment;
        }


        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId) ?? throw new NullReferenceException("Comment Not Found");
            return comment;
        }

        public async Task<List<Comment>> GetCommentsByTaskIdAsync(int taskId)
        {
            return await _context.Comments
                .Where(c => c.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<CommentServiceResponse> DeleteCommentAsync(int commentId)
        {
            var commentToDelete = await _context.Comments.FindAsync(commentId);

            if (commentToDelete == null)
            {
                return new CommentServiceResponse() { IsSucceed = false, Message = "Comment not found." };
            }

            _context.Comments.Remove(commentToDelete);
            await _context.SaveChangesAsync();

            return new CommentServiceResponse() { IsSucceed = true, Message = "Comment deleted successfully." };
        }

    }
}
