using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MessageConsumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        // Establish connection
        var factory = new ConnectionFactory(){HostName= "localhost"};
        var connection = factory.CreateConnection();

        // To Create the communication channel.
        IModel channel =  connection.CreateModel();

        string queue_name = "demo-queue";

        channel.QueueDeclare(queue: queue_name, durable: true, exclusive: false, 
        autoDelete: false,arguments: null); 

        var _consumer = new EventingBasicConsumer(channel);

        _consumer.Received += (p1, p2)=>{
            var body = p2.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine("Message Received: " + message);
        };


        channel.BasicConsume(queue: queue_name, autoAck: true, _consumer = _consumer);
    }
}
