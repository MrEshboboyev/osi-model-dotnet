using Shared.Models;

namespace Shared.Layers;

public class NetworkLayer : IOsiLayer
{
    public int LayerNumber => 3;
    public string LayerName => "Network";
    public string Description => "Handles routing and logical addressing (IP)";

    public OsiLayerData ProcessData(string data, string sourceIp, string destinationIp)
    {
        string packetData = $"[PACKET_HDR][SRC_IP:{sourceIp}][DST_IP:{destinationIp}]{data}[/PACKET]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = packetData,
            Description = Description
        };
    }

    public OsiLayerData ProcessData(string data)
    {
        // Default IP addresses for backward compatibility
        return ProcessData(data, "0.0.0.0", "255.255.255.255");
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove packet headers
        if (data.StartsWith("[PACKET_HDR]") && data.Contains("[/PACKET]"))
        {
            // Extract source and destination IP addresses from the packet
            int srcStart = data.IndexOf("[SRC_IP:") + 8;
            int srcEnd = data.IndexOf(']', srcStart);
            int dstStart = data.IndexOf("[DST_IP:") + 8;
            int dstEnd = data.IndexOf(']', dstStart);
            _ = srcStart > 7 && srcEnd > srcStart ? data[srcStart..srcEnd] : "Unknown";
            _ = dstStart > 7 && dstEnd > dstStart ? data[dstStart..dstEnd] : "Unknown";

            int startIndex = data.IndexOf(']', dstEnd) + 2; // Start after the DST_IP value and closing bracket
            int endIndex = data.IndexOf("[/PACKET]");
            if (endIndex > startIndex)
            {
                string extractedData = data[startIndex..endIndex];
                return extractedData;
            }
        }
        return data;
    }
}
