using Shared.Models;

namespace Shared.Layers;

public class ApplicationLayer : IOsiLayer
{
    public int LayerNumber => 7;
    public string LayerName => "Application";
    public string Description => "Provides network services directly to end-user applications";

    public OsiLayerData ProcessData(string data)
    {
        string applicationData = $"[APP]{data}[/APP]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = applicationData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove application headers
        if (data.StartsWith("[APP]") && data.Contains("[/APP]"))
        {
            int startIndex = "[APP]".Length;
            int endIndex = data.IndexOf("[/APP]");
            if (endIndex > startIndex)
            {
                return data[startIndex..endIndex];
            }
        }
        return data;
    }
}
