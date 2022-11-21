using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

Console.WriteLine("Enter the queue name:");

string queueName = Console.ReadLine();

channel.ExchangeDeclare("publisher", "fanout", false, false);
channel.QueueDeclare(queue: queueName, true, false, false);
channel.QueueBind(queue: queueName, exchange: "publisher", routingKey: "");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, arg) =>
{
    string message = Encoding.UTF8.GetString(arg.Body.ToArray());
    channel.BasicAck(arg.DeliveryTag, false);
    Console.WriteLine($"Subscriber: [{queueName}] message: {message}");
};

channel.BasicConsume(queueName, false, consumer);

Console.ReadKey();
