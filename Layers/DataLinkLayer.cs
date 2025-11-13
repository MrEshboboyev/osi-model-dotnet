using OsiModelDemo.Models;

namespace OsiModelDemo.Layers;

public class DataLinkLayer : IOsiLayer
{
    public int LayerNumber => 2;
    public string LayerName => "Data Link";
    public string Description => "Packages data into frames with MAC addresses";

    public OsiLayerData ProcessData(string data)
    {
        string framedData = $"[FRAME_HDR][SRC_MAC:AA-BB-CC-DD-EE-FF][DST_MAC:11-22-33-44-55-66]{data}[FCS:1234][/FRAME]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = framedData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove frame headers and trailers
        if (data.StartsWith("[FRAME_HDR]") && data.Contains("[/FRAME]"))
        {
            int startIndex = "[FRAME_HDR][SRC_MAC:AA-BB-CC-DD-EE-FF][DST_MAC:11-22-33-44-55-66]".Length;
            int endIndex = data.IndexOf("[FCS:1234][/FRAME]");
            if (endIndex > startIndex)
            {
                return data.Substring(startIndex, endIndex - startIndex);
            }
        }
        return data;
    }
}
