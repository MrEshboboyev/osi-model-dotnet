using Newtonsoft.Json;
using Shared.Models;

namespace Shared.Services;

public class OsiVisualizationService
{
    public void VisualizeForwardFlow(List<OsiLayerData> layersData, string systemName)
    {
        Console.WriteLine($"\n=== Forward Flow Through OSI Layers ({systemName}) ===");
        
        // Display from top to bottom (Layer 7 to Layer 1)
        for (int i = layersData.Count - 1; i >= 0; i--)
        {
            var layerData = layersData[i];
            ConsoleColor color = GetLayerColor(layerData.LayerNumber);
            
            Console.ForegroundColor = color;
            Console.WriteLine($"\n[{layerData.LayerNumber}] {layerData.LayerName} Layer:");
            Console.WriteLine($"Description: {layerData.Description}");
            Console.WriteLine($"Data: {layerData.Data}");
            Console.ResetColor();
            
            if (i > 0)
            {
                Console.WriteLine("  ↓");
            }
        }
    }

    public void VisualizeReverseFlow(List<OsiLayerData> layersData, string systemName)
    {
        Console.WriteLine($"\n=== Reverse Flow Through OSI Layers ({systemName}) ===");
        
        // Display from bottom to top (Layer 1 to Layer 7)
        for (int i = 0; i < layersData.Count; i++)
        {
            var layerData = layersData[i];
            ConsoleColor color = GetLayerColor(layerData.LayerNumber);
            
            Console.ForegroundColor = color;
            Console.WriteLine($"\n[{layerData.LayerNumber}] {layerData.LayerName} Layer:");
            Console.WriteLine($"Description: {layerData.Description}");
            Console.WriteLine($"Data: {layerData.Data}");
            Console.ResetColor();
            
            if (i < layersData.Count - 1)
            {
                Console.WriteLine("  ↑");
            }
        }
    }

    private ConsoleColor GetLayerColor(int layerNumber)
    {
        return layerNumber switch
        {
            7 => ConsoleColor.Blue,
            6 => ConsoleColor.Magenta,
            5 => ConsoleColor.Cyan,
            4 => ConsoleColor.Yellow,
            3 => ConsoleColor.Green,
            2 => ConsoleColor.Red,
            1 => ConsoleColor.Gray,
            _ => ConsoleColor.White
        };
    }

    public string SerializeLayerData(List<OsiLayerData> layersData)
    {
        return JsonConvert.SerializeObject(layersData);
    }

    public List<OsiLayerData> DeserializeLayerData(string jsonData)
    {
        return JsonConvert.DeserializeObject<List<OsiLayerData>>(jsonData) ?? new List<OsiLayerData>();
    }
}
