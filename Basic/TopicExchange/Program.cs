using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "agreements", type: "topic", true, false);

//QueueA
channel.QueueDeclare(queue: "berlin_agreements", true, false, false);

//QueueB
channel.QueueDeclare(queue: "all_agreements", true, false, false);

//QueueC
channel.QueueDeclare(queue: "headstore_agreements", true, false, false);


channel.QueueBind(queue: "berlin_agreements", exchange: "agreements", routingKey: "agreements.eu.berlin.#");
channel.QueueBind(queue: "all_agreements", exchange: "agreements", routingKey: "agreements.#");
channel.QueueBind(queue: "headstore_agreements", exchange: "agreements", routingKey: "agreements.eu.*.headstore");

channel.BasicPublish(exchange: "agreements", routingKey: "agreements.eu.berlin");
