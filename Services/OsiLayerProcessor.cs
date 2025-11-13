using OsiModelDemo.Layers;
using OsiModelDemo.Models;

namespace OsiModelDemo.Services;

public class OsiLayerProcessor
{
    private readonly List<IOsiLayer> _layers;

    public OsiLayerProcessor()
    {
        _layers =
        [
            new ApplicationLayer(),
            new PresentationLayer(),
            new SessionLayer(),
            new TransportLayer(),
            new NetworkLayer(),
            new DataLinkLayer(),
            new PhysicalLayer()
        ];
    }

    public List<OsiLayerData> ProcessForward(string inputData)
    {
        var dataFlow = new List<OsiLayerData>();
        string currentData = inputData;

        // Process from top to bottom (Layer 7 to Layer 1)
        for (int i = _layers.Count - 1; i >= 0; i--)
        {
            var layer = _layers[i];
            var layerData = layer.ProcessData(currentData);
            dataFlow.Add(layerData);
            currentData = layerData.Data;
        }

        return dataFlow;
    }

    public List<OsiLayerData> ProcessReverse(List<OsiLayerData> forwardData)
    {
        var reverseDataFlow = new List<OsiLayerData>();
        string currentData = forwardData[0].Data; // Start with physical layer data

        // Process from bottom to top (Layer 1 to Layer 7)
        for (int i = 0; i < _layers.Count; i++)
        {
            var layer = _layers[i];
            var layerData = forwardData[forwardData.Count - 1 - i]; // Get corresponding layer data
            string processedData = layer.ReverseProcessData(layerData);
            
            var reverseLayerData = new OsiLayerData
            {
                LayerNumber = layer.LayerNumber,
                LayerName = layer.LayerName,
                Data = processedData,
                Description = layer.Description
            };
            
            reverseDataFlow.Add(reverseLayerData);
            currentData = processedData;
        }

        return reverseDataFlow;
    }
}
