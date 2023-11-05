using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new();

factory.Uri = new("...");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchangeName = "fanout-exchange-example";

channel.ExchangeDeclare(
    exchange: exchangeName,
    type: ExchangeType.Fanout);

Console.Write("please enter a queue name: ");
string queueName = Console.ReadLine();

channel.QueueDeclare(
    queue: queueName,
    exclusive: false //false because of maybe other consumers want to consume this queue's data
    );
channel.QueueBind(
    queue: queueName,
    exchange: exchangeName,
    routingKey: string.Empty);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true, // rabbitMQ wants from us acknowledge message, If we enter true this is automatically send message after done queue's duties
    consumer: consumer );
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();