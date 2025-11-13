using Shared.Layers;
using Shared.Models;
using Shared.Services;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Server;

public class OsiServer
{
    private readonly List<IOsiLayer> _layers;
    private readonly OsiVisualizationService _visualizationService;
    private readonly string _macAddress;
    private readonly string _ipAddress;

    public OsiServer()
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

    public async Task StartAsync(int port)
    {
        var listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"OSI Model Server listening on port {port}");
        Console.WriteLine($"Server MAC Address: {_macAddress}");
        Console.WriteLine($"Server IP Address: {_ipAddress}");
        Console.WriteLine("Waiting for client connection...\n");

        try
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        Console.WriteLine($"Client connected from {((IPEndPoint?)client.Client.RemoteEndPoint)?.Address}");

        try
        {
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            // Read incoming data
            string? jsonData = await reader.ReadLineAsync();
            jsonData ??= "";
            var receivedLayers = _visualizationService.DeserializeLayerData(jsonData);

            // Visualize received data
            _visualizationService.VisualizeForwardFlow(receivedLayers, "Received from Client");

            // Extract original message
            string message = ExtractMessage(receivedLayers);
            Console.WriteLine($"\n📝 Extracted message: \"{message}\"");

            // Reverse the message
            string reversedMessage = ReverseString(message);
            Console.WriteLine($"🔄 Reversed message: \"{reversedMessage}\"");

            // Process response through OSI layers (reverse direction)
            var responseLayers = ProcessThroughLayers(reversedMessage);

            // Visualize response
            _visualizationService.VisualizeReverseFlow(responseLayers, "Response to Client");

            // Send response back to client
            string responseJson = _visualizationService.SerializeLayerData(responseLayers);
            await writer.WriteLineAsync(responseJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("Client disconnected\n");
        }
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
                DataLinkLayer dataLinkLayer => dataLinkLayer.ProcessData(currentData, _macAddress, "Client-MAC"),// In real scenario, would get client MAC
                NetworkLayer networkLayer => networkLayer.ProcessData(currentData, _ipAddress, "Client-IP"),// In real scenario, would get client IP
                TransportLayer transportLayer => transportLayer.ProcessData(currentData, 23673, 33782),// Server port, client port
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

    private static string ReverseString(string input)
    {
        return new string([.. input.Reverse()]);
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
            return "AA-BB-CC-DD-EE-FF";
        }
        
        return "AA-BB-CC-DD-EE-FF";
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
