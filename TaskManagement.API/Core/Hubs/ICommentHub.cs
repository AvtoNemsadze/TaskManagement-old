namespace TaskManagement.API.Core.Hubs
{
    public interface ICommentHub
    {
        Task ReceiveTaskCommentNotification(int taskId, string commentText);
    }
}
