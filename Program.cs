using OsiModelDemo.Services;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "OSI Model Data Flow Demonstration";

var processor = new OsiLayerProcessor();
var visualizer = new OsiVisualizationService();

DisplayWelcomeMessage();

while (true)
{
    Console.WriteLine("\n" + new string('=', 80));
    Console.Write("Enter text to demonstrate through OSI layers (or 'quit' to exit): ");
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) || input.Equals("quit", StringComparison.CurrentCultureIgnoreCase))
        break;

    Console.Clear();

    // Forward direction (Application to Physical)
    var forwardData = processor.ProcessForward(input);
    visualizer.DisplayForwardProcess(forwardData);

    Console.WriteLine("\n" + new string('-', 80));

    // Reverse direction (Physical to Application)
    var reverseData = processor.ProcessReverse(forwardData);
    visualizer.DisplayReverseProcess(reverseData);

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
    Console.Clear();
    DisplayWelcomeMessage();
}

Console.WriteLine("Thank you for using the OSI Model Demo!");


static void DisplayWelcomeMessage()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(@"
                          ____  ____ _____      __  __ 
                         / __ \| ___|_ _\ \    / /  \/  |
                        | |  | |___ \| | \ \  / /| |\/| |
                        | |__| |___) | |  \ \/ / | |  | |
                         \____/|____/___|  \__/  |_|  |_|
                        ");
    Console.ResetColor();
    Console.WriteLine("This application demonstrates how data flows through the 7 layers of the OSI model.");
    Console.WriteLine("Each layer adds its own header (and sometimes trailer) to the data.");
    Console.WriteLine("The process is shown both for sending data (forward) and receiving data (reverse).\n");
}
