using Shared.Models;

namespace Shared.Layers;

public class PhysicalLayer : IOsiLayer
{
    public int LayerNumber => 1;
    public string LayerName => "Physical";
    public string Description => "Transmits raw bit stream over physical medium";

    public OsiLayerData ProcessData(string data)
    {
        // Convert string to binary representation
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
        string data = layerData.Data;
        // Convert binary back to string
        try
        {
            return BinaryToString(data);
        }
        catch
        {
            return data; // Return as-is if not valid binary
        }
    }

    private static string StringToBinary(string text)
    {
        return string.Join(" ", text.Select(c => Convert.ToString(c, 2).PadLeft(8, '0')));
    }

    private static string BinaryToString(string binary)
    {
        // Remove spaces and convert back to string
        string cleanBinary = binary.Replace(" ", "");
        if (cleanBinary.Length % 8 != 0)
            throw new ArgumentException("Invalid binary string");

        byte[] bytes = new byte[cleanBinary.Length / 8];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(cleanBinary.Substring(i * 8, 8), 2);
        }
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
