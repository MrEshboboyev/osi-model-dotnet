using Client;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "OSI Model Client Service";

var client = new OsiClient();
await client.StartAsync("127.0.0.1", 23673); // Connect to port 23673
