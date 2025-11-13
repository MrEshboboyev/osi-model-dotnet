using OsiModelDemo.Models;

namespace OsiModelDemo.Layers;

public interface IOsiLayer
{
    int LayerNumber { get; }
    string LayerName { get; }
    string Description { get; }
    OsiLayerData ProcessData(string data);
    string ReverseProcessData(OsiLayerData layerData);
}
