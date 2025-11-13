using OsiModelDemo.Models;

namespace OsiModelDemo.Services;

public class OsiVisualizationService
{
    public void DisplayForwardProcess(List<OsiLayerData> dataFlow)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("FORWARD DIRECTION (Sending Data)");
        Console.WriteLine(new string('=', 80));
        Console.ResetColor();

        // Display from Layer 7 to Layer 1 (reverse order of the list)
        for (int i = dataFlow.Count - 1; i >= 0; i--)
        {
            var layerData = dataFlow[i];
            ConsoleColor color = GetLayerColor(layerData.LayerNumber);
            
            Console.ForegroundColor = color;
            Console.WriteLine($"[Layer {layerData.LayerNumber} - {layerData.LayerName}] {layerData.Description}");
            Console.ResetColor();
            
            if (i < dataFlow.Count - 1)
            {
                Console.WriteLine($"  Received data: {dataFlow[i + 1].Data}");
            }
            else
            {
                Console.WriteLine($"  Original data: \"{ExtractOriginalData(dataFlow[i].Data)}\"");
            }
            
            Console.WriteLine($"  Processed data: {layerData.Data}");
            Console.WriteLine();
        }
    }

    public void DisplayReverseProcess(List<OsiLayerData> dataFlow)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("REVERSE DIRECTION (Receiving Data)");
        Console.WriteLine(new string('=', 80));
        Console.ResetColor();

        // Display from Layer 1 to Layer 7
        for (int i = 0; i < dataFlow.Count; i++)
        {
            var layerData = dataFlow[i];
            ConsoleColor color = GetLayerColor(layerData.LayerNumber);
            
            Console.ForegroundColor = color;
            Console.WriteLine($"[Layer {layerData.LayerNumber} - {layerData.LayerName}] {layerData.Description}");
            Console.ResetColor();
            
            if (i > 0)
            {
                Console.WriteLine($"  Received data: {dataFlow[i - 1].Data}");
            }
            else
            {
                Console.WriteLine($"  Received data: {layerData.Data}");
            }
            
            Console.WriteLine($"  Processed data: {layerData.Data}");
            Console.WriteLine();
        }
        
        // Display final result
        if (dataFlow.Count > 0)
        {
            var finalData = dataFlow[^1];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"✅ Successfully received and processed: \"{ExtractOriginalData(finalData.Data)}\"");
            Console.ResetColor();
        }
    }

    private ConsoleColor GetLayerColor(int layerNumber)
    {
        return layerNumber switch
        {
            1 => ConsoleColor.DarkGray,
            2 => ConsoleColor.DarkGreen,
            3 => ConsoleColor.DarkCyan,
            4 => ConsoleColor.Red,
            5 => ConsoleColor.DarkYellow,
            6 => ConsoleColor.Magenta,
            7 => ConsoleColor.Blue,
            _ => ConsoleColor.White
        };
    }

    private static string ExtractOriginalData(string data)
    {
        // Simple extraction for display purposes
        if (data.Contains("[APP]") && data.Contains("[/APP]"))
        {
            int start = data.IndexOf("[APP]") + 5;
            int end = data.IndexOf("[/APP]");
            if (start < end)
            {
                return data[start..end];
            }
        }
        return data.Length > 50 ? string.Concat(data.AsSpan(0, 50), "...") : data;
    }
}
