using Server;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "OSI Model Server Service";

var server = new OsiServer();
await server.StartAsync(23673); // Using port 23673 as requested
