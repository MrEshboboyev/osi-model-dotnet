using OsiModelDemo.Models;

namespace OsiModelDemo.Layers;

public class TransportLayer : IOsiLayer
{
    public int LayerNumber => 4;
    public string LayerName => "Transport";
    public string Description => "Segments data and manages end-to-end communication (TCP/UDP)";

    public OsiLayerData ProcessData(string data)
    {
        string segmentData = $"[TCP HDR][PORT:80]{data}[SEQ:100][ACK:200][/TCP]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = segmentData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove transport headers
        if (data.StartsWith("[TCP HDR]") && data.Contains("[/TCP]"))
        {
            int startIndex = "[TCP HDR][PORT:80]".Length;
            int endIndex = data.IndexOf("[SEQ:100][ACK:200][/TCP]");
            if (endIndex > startIndex)
            {
                return data.Substring(startIndex, endIndex - startIndex);
            }
        }
        return data;
    }
}
