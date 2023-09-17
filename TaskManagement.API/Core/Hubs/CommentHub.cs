using Microsoft.AspNetCore.SignalR;
using TaskManagement.API.Core.Hubs;

namespace TaskManagement.API.Core.Hubs
{
    public class CommentHub : Hub<ICommentHub>
    {
        public async Task SendTaskCommentNotification(int taskId, string commentText)
        {
            await Clients.User(Context.UserIdentifier).ReceiveTaskCommentNotification(taskId, commentText);
        }
    }
}
