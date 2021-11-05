using System;
using MQTTnet;
using static System.Console;
using MQTTnet.Client.Options;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Extensions.ManagedClient;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

WriteLine("Start App");

var _url = "";
var _topic = "topic/json";
var _port = 8200;
do
{
    Write("Site Address:");
    _url = ReadLine();
} while (string.IsNullOrWhiteSpace(_url));
do
{
    Write("Port number:");

} while (!int.TryParse(ReadLine(), out _port));
WriteLine("Enter topic( if empty use defualt topic):");
var _tempTopic = ReadLine();
if (string.IsNullOrWhiteSpace(_tempTopic))
{
    WriteLine("use defualt topic: " + _topic);
}
else
    _topic = _tempTopic;
var builder = new MqttClientOptionsBuilder()
    .WithClientId(Guid.NewGuid().ToString())
    .WithTcpServer(_url, _port);
var option = new ManagedMqttClientOptionsBuilder().WithAutoReconnectDelay(TimeSpan.FromSeconds(50)).WithClientOptions(builder.Build()).Build();
var client = new MqttFactory().CreateManagedMqttClient();
client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
client.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
await client.StartAsync(option);
while (true)
{
    string json = JsonSerializer.Serialize(new { message = "Heyo :)", sent = DateTimeOffset.UtcNow });
    await client.PublishAsync(_topic, json);
    await Task.Delay(1000);
}
static void OnConnected(MqttClientConnectedEventArgs obj)
{
    WriteLine("Successfully connected.");
}

static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
{
    WriteLine("Couldn't connect to broker.");
}

static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
{
    WriteLine("Successfully disconnected.");
}