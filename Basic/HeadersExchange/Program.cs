using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "agreements", type: "headers", true, false);

channel.QueueDeclare(queue: "QueueA", true, false, false);
channel.QueueDeclare(queue: "QueueB", true, false, false);
channel.QueueDeclare(queue: "QueueC", true, false, false);

var argument1 = new Dictionary<string, object>
{
    { "format", "pdf" },
    { "type", "report" },
    { "x-match", "all" }
};
channel.QueueBind(queue: "QueueA", exchange: "agreements", routingKey: "", argument1);

var argument2 = new Dictionary<string, object>
{
    { "format", "pdf" },
    { "type", "log" },
    { "x-match", "any" }
};
channel.QueueBind(queue: "QueueB", exchange: "agreements", routingKey: "", argument2);

var argument3 = new Dictionary<string, object>
{
    { "format", "zip" },
    { "type", "report" },
    { "x-match", "all" }
};
channel.QueueBind(queue: "QueueC", exchange: "agreements", routingKey: "", argument3);

//Publish 1
var property1 = channel.CreateBasicProperties();
property1.Headers = new Dictionary<string, object>();
property1.Headers.Add("format", "pdf");
property1.Headers.Add("type", "report");
channel.BasicPublish(exchange: "agreements", routingKey: "", property1, Encoding.UTF8.GetBytes("Hello 1"));

//Publish 2
var property2 = channel.CreateBasicProperties();
property2.Headers = new Dictionary<string, object>();
property2.Headers.Add("format", "pdf");
channel.BasicPublish(exchange: "agreements", routingKey: "", property2, Encoding.UTF8.GetBytes("Hello 2"));

//Publish 3
var property3 = channel.CreateBasicProperties();
property3.Headers = new Dictionary<string, object>();
property3.Headers.Add("format", "zip");
property3.Headers.Add("type", "log");
channel.BasicPublish(exchange: "agreements", routingKey: "", property3, Encoding.UTF8.GetBytes("Hello 3"));
