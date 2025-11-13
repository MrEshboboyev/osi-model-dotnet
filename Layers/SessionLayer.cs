using OsiModelDemo.Models;

namespace OsiModelDemo.Layers;

public class SessionLayer : IOsiLayer
{
    public int LayerNumber => 5;
    public string LayerName => "Session";
    public string Description => "Manages sessions and dialogues between applications";

    public OsiLayerData ProcessData(string data)
    {
        string sessionData = $"[SES]{data}[SID:12345]";
        
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
        // Remove session identifiers
        if (data.StartsWith("[SES]") && data.Contains("[SID:12345]"))
        {
            int startIndex = "[SES]".Length;
            int endIndex = data.IndexOf("[SID:12345]");
            if (endIndex > startIndex)
            {
                return data[startIndex..endIndex];
            }
        }
        return data;
    }
}
