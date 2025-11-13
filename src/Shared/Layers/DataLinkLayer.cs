using Shared.Models;

namespace Shared.Layers;

public class DataLinkLayer : IOsiLayer
{
    public int LayerNumber => 2;
    public string LayerName => "Data Link";
    public string Description => "Packages data into frames with MAC addresses";

    public OsiLayerData ProcessData(string data, string sourceMac, string destinationMac)
    {
        string framedData = $"[FRAME_HDR][SRC_MAC:{sourceMac}][DST_MAC:{destinationMac}]{data}[FCS:1234][/FRAME]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = framedData,
            Description = Description
        };
    }

    public OsiLayerData ProcessData(string data)
    {
        // Default MAC addresses for backward compatibility
        return ProcessData(data, "00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF");
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove frame headers and trailers
        if (data.StartsWith("[FRAME_HDR]") && data.Contains("[/FRAME]"))
        {
            // Extract source and destination MAC addresses from the frame
            int srcStart = data.IndexOf("[SRC_MAC:") + 9;
            int srcEnd = data.IndexOf("]", srcStart);
            int dstStart = data.IndexOf("[DST_MAC:") + 9;
            int dstEnd = data.IndexOf("]", dstStart);
            _ = srcStart > 8 && srcEnd > srcStart
                ? data[srcStart..srcEnd]
                : "Unknown";
            _ = dstStart > 8 && dstEnd > dstStart 
                ? data[dstStart..dstEnd] 
                : "Unknown";

            int startIndex = data.IndexOf(']', dstEnd) + 2; // Start after the DST_MAC value and closing bracket
            int endIndex = data.IndexOf("[FCS:1234][/FRAME]");
            if (endIndex > startIndex)
            {
                string extractedData = data[startIndex..endIndex];
                return extractedData;
            }
        }
        return data;
    }
}
