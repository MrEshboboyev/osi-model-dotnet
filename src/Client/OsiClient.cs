using Shared.Layers;
using Shared.Models;
using Shared.Services;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Client;

public class OsiClient
{
    private readonly List<IOsiLayer> _layers;
    private readonly OsiVisualizationService _visualizationService;
    private readonly string _macAddress;
    private readonly string _ipAddress;

    public OsiClient()
    {
        // Initialize layers
        _layers =
        [
            new PhysicalLayer(),
            new DataLinkLayer(),
            new NetworkLayer(),
            new TransportLayer(),
            new SessionLayer(),
            new PresentationLayer(),
            new ApplicationLayer()
        ];

        _visualizationService = new OsiVisualizationService();
        
        // Get system information
        _macAddress = GetMacAddress();
        _ipAddress = GetLocalIPAddress();
    }

    public async Task StartAsync(string serverIp, int port)
    {
        try
        {
            using var client = new TcpClient();
            Console.WriteLine($"Connecting to server at {serverIp}:{port}...");
            Console.WriteLine($"Client MAC Address: {_macAddress}");
            Console.WriteLine($"Client IP Address: {_ipAddress}");

            await client.ConnectAsync(serverIp, port);
            Console.WriteLine("✅ Connected to server\n");

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            // Get message from user
            Console.Write("Enter message to send: ");
            string? message = Console.ReadLine();
            message ??= "Hello OSI Model!";

            // Process message through OSI layers
            var layersData = ProcessThroughLayers(message);

            // Visualize the forward flow
            _visualizationService.VisualizeForwardFlow(layersData, "Client Sending");

            // Send data to server
            string jsonData = _visualizationService.SerializeLayerData(layersData);
            await writer.WriteLineAsync(jsonData);

            // Read response from server
            string? responseJson = await reader.ReadLineAsync();
            responseJson ??= "";
            var responseLayers = _visualizationService.DeserializeLayerData(responseJson);

            // Visualize the reverse flow
            _visualizationService.VisualizeReverseFlow(responseLayers, "Client Receiving");

            // Extract final message
            string finalMessage = ExtractMessage(responseLayers);
            Console.WriteLine($"\n✅ Final received message: \"{finalMessage}\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    private List<OsiLayerData> ProcessThroughLayers(string data)
    {
        var layersData = new List<OsiLayerData>();
        string currentData = data;

        // Process through layers 7 to 1 (forward direction)
        for (int i = _layers.Count - 1; i >= 0; i--)
        {
            var layer = _layers[i];

            // Special handling for layers that need additional parameters
            OsiLayerData layerData = layer switch
            {
                DataLinkLayer dataLinkLayer => dataLinkLayer.ProcessData(currentData, _macAddress, "Server-MAC"),// In real scenario, would get server MAC
                NetworkLayer networkLayer => networkLayer.ProcessData(currentData, _ipAddress, "Server-IP"),// In real scenario, would get server IP
                TransportLayer transportLayer => transportLayer.ProcessData(currentData, 33782, 23673),// Client port, server port
                _ => layer.ProcessData(currentData),
            };
            layersData.Add(layerData);
            currentData = layerData.Data;
        }

        return layersData;
    }

    private string ExtractMessage(List<OsiLayerData> layersData)
    {
        // Start from the bottom layer (physical) and work up
        string currentData = layersData[0].Data; // Physical layer data

        // Process through layers 1 to 7 (reverse direction)
        for (int i = 0; i < _layers.Count; i++)
        {
            var layer = _layers[i];
            var layerData = layersData.FirstOrDefault(ld => ld.LayerNumber == layer.LayerNumber);
            if (layerData != null)
            {
                currentData = layer.ReverseProcessData(layerData);
            }
        }

        return currentData;
    }

    private static string GetMacAddress()
    {
        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                             nic.NetworkInterfaceType != NetworkInterfaceType.Loopback);

            foreach (var nic in networkInterfaces)
            {
                var mac = nic.GetPhysicalAddress();
                if (mac != null)
                {
                    return string.Join("-", mac.GetAddressBytes().Select(b => b.ToString("X2")));
                }
            }
        }
        catch
        {
            // Return a default MAC if unable to get system MAC
            return "11-22-33-44-55-66";
        }
        
        return "11-22-33-44-55-66";
    }

    private static string GetLocalIPAddress()
    {
        try
        {
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
            return endPoint?.Address.ToString() ?? "127.0.0.1";
        }
        catch
        {
            return "127.0.0.1";
        }
    }
}
