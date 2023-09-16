using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7211/taskHub") // Replace with your server URL and hub endpoint
            .Build();

        hubConnection.On<string>("TaskCreated", (newTask) =>
        {
            Console.WriteLine($"New task created: {newTask}");
            // Handle the task creation event as needed
        });

        await hubConnection.StartAsync();

        Console.WriteLine("Connected to SignalR hub. Press Enter to exit.");
        Console.ReadLine();

        await hubConnection.StopAsync();
    }
}

