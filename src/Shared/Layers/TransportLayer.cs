using Shared.Models;

namespace Shared.Layers;

public class TransportLayer : IOsiLayer
{
    public int LayerNumber => 4;
    public string LayerName => "Transport";
    public string Description => "Ensures complete data transfer with error recovery and flow control";

    public OsiLayerData ProcessData(string data, int sourcePort, int destinationPort)
    {
        string segmentData = $"[SEGMENT_HDR][SRC_PORT:{sourcePort}][DST_PORT:{destinationPort}]{data}[SEQ:0001][ACK:0000][/SEGMENT]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = segmentData,
            Description = Description
        };
    }

    public OsiLayerData ProcessData(string data)
    {
        // Default ports for backward compatibility
        return ProcessData(data, 1024, 80);
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove transport headers
        if (data.StartsWith("[SEGMENT_HDR]") && data.Contains("[/SEGMENT]"))
        {
            // Extract source and destination ports from the segment
            int srcStart = data.IndexOf("[SRC_PORT:") + 10;
            int srcEnd = data.IndexOf(']', srcStart);
            int dstStart = data.IndexOf("[DST_PORT:") + 10;
            int dstEnd = data.IndexOf(']', dstStart);
            _ = srcStart > 9 && srcEnd > srcStart ? data[srcStart..srcEnd] : "Unknown";
            _ = dstStart > 9 && dstEnd > dstStart ? data[dstStart..dstEnd] : "Unknown";

            int startIndex = data.IndexOf(']', dstEnd) + 2; // Start after the DST_PORT value and closing bracket
            int endIndex = data.IndexOf("[SEQ:0001][ACK:0000][/SEGMENT]");
            if (endIndex > startIndex)
            {
                string extractedData = data[startIndex..endIndex];
                return extractedData;
            }
        }
        return data;
    }
}
