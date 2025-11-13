using OsiModelDemo.Models;
using System.Text;

namespace OsiModelDemo.Layers;

public class PresentationLayer : IOsiLayer
{
    public int LayerNumber => 6;
    public string LayerName => "Presentation";
    public string Description => "Translates, encrypts, and compresses data";

    public OsiLayerData ProcessData(string data)
    {
        // Simple encoding for demonstration
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        string encodedData = Convert.ToBase64String(bytes);
        string presentationData = $"[PRES]{encodedData}[/PRES]";
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = presentationData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        string data = layerData.Data;
        // Remove presentation headers and decode
        if (data.StartsWith("[PRES]") && data.Contains("[/PRES]"))
        {
            int startIndex = "[PRES]".Length;
            int endIndex = data.IndexOf("[/PRES]");
            if (endIndex > startIndex)
            {
                string encodedData = data[startIndex..endIndex];
                try
                {
                    byte[] bytes = Convert.FromBase64String(encodedData);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch
                {
                    return "[Unable to decode presentation data]";
                }
            }
        }
        return data;
    }
}
