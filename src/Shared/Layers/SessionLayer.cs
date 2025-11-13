using Shared.Models;

namespace Shared.Layers;

public class SessionLayer : IOsiLayer
{
    public int LayerNumber => 5;
    public string LayerName => "Session";
    public string Description => "Establishes, manages, and terminates connections between applications";

    public OsiLayerData ProcessData(string data)
    {
        // Add session identifier
        string sessionData = $"[SESSION_ID:{Guid.NewGuid()}]{data}[/SESSION]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = sessionData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove session headers
        if (data.StartsWith("[SESSION_ID:") && data.Contains("[/SESSION]"))
        {
            int startIndex = data.IndexOf(']') + 1;
            int endIndex = data.IndexOf("[/SESSION]");
            if (endIndex > startIndex)
            {
                return data[startIndex..endIndex];
            }
        }
        return data;
    }
}
