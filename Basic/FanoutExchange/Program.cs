using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "sport_news", type: "fanout", true, false);

channel.QueueDeclare(queue: "QueueA", true, false, false);
channel.QueueDeclare(queue: "QueueB", true, false, false);
channel.QueueDeclare(queue: "QueueC", true, false, false);

channel.QueueBind(queue: "QueueA", exchange: "sport_news", routingKey: "");
channel.QueueBind(queue: "QueueB", exchange: "sport_news", routingKey: "");
channel.QueueBind(queue: "QueueC", exchange: "sport_news", routingKey: "");

channel.BasicPublish(exchange: "sport_news", routingKey: "");

