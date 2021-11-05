using MQTTnet.Server;
using System;
using MQTTnet;
using System.Text;
using static System.Console;
using System.Threading;
using static SharedItems.LogMessages;

var wait = new ManualResetEvent(false);
var option = new MqttServerOptionsBuilder().WithDefaultEndpoint()
    .WithDefaultEndpointPort(1883)
    .WithConnectionValidator(OnNewConnection)
    .WithApplicationMessageInterceptor(OnNewMessage);
var mqttServer = new MqttFactory().CreateMqttServer();
await mqttServer.StartAsync(option.Build());
ReadLine();
wait.WaitOne();
