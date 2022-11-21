using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("publisher", "fanout", false, false);

while (true)
{
    Console.WriteLine("Enter message:");
    string message = Console.ReadLine();

    channel.BasicPublish("publisher", "", null, Encoding.UTF8.GetBytes(message));
}

