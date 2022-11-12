using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("pdf_events", "direct", true);

channel.QueueDeclare(queue: "create_pdf_queue", false, false, false, null);
channel.QueueDeclare(queue: "pdf_log_queue", false, false, false, null);

channel.QueueBind(queue: "create_pdf_queue", exchange: "pdf_events", routingKey: "pdf_create");
channel.QueueBind(queue: "pdf_log_queue", exchange: "pdf_events", routingKey: "pdf_log");

channel.BasicPublish(exchange: "pdf_events", routingKey: "pdf_log");
