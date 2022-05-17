// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;

var uri = "http://localhost:5001/streamHub";

await using var connection = new HubConnectionBuilder().WithUrl(uri).Build();

await connection.StartAsync();

await foreach (var date in connection.StreamAsync<int>("Streaming"))
{
    Console.WriteLine(date);
}