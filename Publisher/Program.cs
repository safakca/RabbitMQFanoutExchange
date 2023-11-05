using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new();

factory.Uri = new("...");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchangeName = "fanout-exchange-example";

channel.ExchangeDeclare(
    exchange: exchangeName,
    type: ExchangeType.Fanout);

for (int i = 0; i < 100 ; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i}");
    channel.BasicPublish(
        exchange: exchangeName,
        routingKey: string.Empty,
        body: message);
}

Console.Read();