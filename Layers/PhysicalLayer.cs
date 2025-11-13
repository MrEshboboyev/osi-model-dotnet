using OsiModelDemo.Models;
using System.Text;

namespace OsiModelDemo.Layers;

public class PhysicalLayer : IOsiLayer
{
    public int LayerNumber => 1;
    public string LayerName => "Physical";
    public string Description => "Converts data to electrical/optical signals for transmission";

    public OsiLayerData ProcessData(string data)
    {
        // Convert to binary representation for visualization
        string binaryData = StringToBinary(data);
        
        return new OsiLayerData
        {
            LayerNumber = LayerNumber,
            LayerName = LayerName,
            Data = binaryData,
            Description = Description
        };
    }

    public string ReverseProcessData(OsiLayerData layerData)
    {
        // Convert binary back to string
        return BinaryToString(layerData.Data);
    }

    private static string StringToBinary(string data)
    {
        StringBuilder sb = new();
        foreach (char c in data)
        {
            sb.Append(System.Convert.ToString(c, 2).PadLeft(8, '0'));
            sb.Append(' ');
        }
        return sb.ToString().Trim();
    }

    private static string BinaryToString(string binary)
    {
        try
        {
            string[] binaryValues = binary.Split(' ');
            StringBuilder sb = new();
            foreach (string binaryValue in binaryValues)
            {
                if (!string.IsNullOrEmpty(binaryValue))
                {
                    int value = Convert.ToInt32(binaryValue, 2);
                    sb.Append((char)value);
                }
            }
            return sb.ToString();
        }
        catch
        {
            return "[Unable to decode binary]";
        }
    }
}
