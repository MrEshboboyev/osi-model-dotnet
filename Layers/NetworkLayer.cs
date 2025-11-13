using OsiModelDemo.Models;

namespace OsiModelDemo.Layers;

public class NetworkLayer : IOsiLayer
{
    public int LayerNumber => 3;
    public string LayerName => "Network";
    public string Description => "Handles routing and logical addressing (IP)";

    public OsiLayerData ProcessData(string data)
    {
        string packetData = $"[IP HDR][SRC:192.168.1.100][DST:8.8.8.8]{data}[CRC:ABCD][/IP]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = packetData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove IP headers
        if (data.StartsWith("[IP HDR]") && data.Contains("[/IP]"))
        {
            int startIndex = "[IP HDR][SRC:192.168.1.100][DST:8.8.8.8]".Length;
            int endIndex = data.IndexOf("[CRC:ABCD][/IP]");
            if (endIndex > startIndex)
            {
                return data[startIndex..endIndex];
            }
        }
        return data;
    }
}
