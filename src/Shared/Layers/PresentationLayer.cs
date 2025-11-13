using Shared.Models;
using System.Text;

namespace Shared.Layers;

public class PresentationLayer : IOsiLayer
{
    public int LayerNumber => 6;
    public string LayerName => "Presentation";
    public string Description => "Translates data between application and network formats";

    public OsiLayerData ProcessData(string data)
    {
        // Convert to Base64 for encoding demonstration
        string encodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = encodedData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Decode from Base64
        try
        {
            byte[] decodedBytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(decodedBytes);
        }
        catch
        {
            return data; // Return as-is if not valid Base64
        }
    }
}
