using Microsoft.AspNetCore.SignalR.Client;

namespace TaskManagement.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7211/commentHub") // Replace with your SignalR hub URL
                .Build();

            hubConnection.On<int, string>("ReceiveTaskCommentNotification", (taskId, commentText) =>
            {
                Console.WriteLine($"New comment added to task {taskId}: {commentText}");
                // Handle the notification as needed
            });

            await hubConnection.StartAsync();

            Console.WriteLine("Connected to SignalR hub1. Press Enter to exit.");
            Console.ReadLine();

            await hubConnection.StopAsync();
        }
    }
}
