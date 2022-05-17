// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;

var uri = "http://localhost:5002/current-time";

await using var connection = new HubConnectionBuilder().WithUrl(uri).Build();

await connection.StartAsync();

await foreach (var date in connection.StreamAsync<DateTime>("Streaming"))
{
    Console.WriteLine(date);
}