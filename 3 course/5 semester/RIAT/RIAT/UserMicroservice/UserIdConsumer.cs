using RabbitMQ.Client;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using UserMicroservice.Context;

//RabbitMQ Consumer
public class UserIdConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    
    private IConnection _connection;
    private IModel _channel;
    private string _queueRequest;
    private string _queueResponse;

    // Constructor: initializes the RabbitMQ connection and declares the queue
    public UserIdConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        
        // Setting up the RabbitMQ factory from the configuration
        _queueRequest = _configuration["RabbitMQ:RequestQueueUId"]!;
        _queueResponse = _configuration["RabbitMQ:ResponseQueueUId"]!; 
        
        var factory = _configuration.GetSection("RabbitMQ").Get<ConnectionFactory>();
        _connection = factory!.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare the queue that this service will consume from
        _channel.QueueDeclare(queue: _queueRequest, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: _queueResponse, durable: true, exclusive: false, autoDelete: false, arguments: null);

        // Set QoS (Quality of Service) to limit how many messages can be sent to a consumer at a time
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);
    }

    // Main processing method to consume messages
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var request = JsonConvert.DeserializeObject<dynamic>(message);
            var username = (string)request.Username;
            
            // Get the replyTo address and correlationId for sending the response
            var replyTo = ea.BasicProperties.ReplyTo;
            var correlationId = ea.BasicProperties.CorrelationId;

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            
            // Find user by username in the database (async operation)
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: stoppingToken);
            var userId = user?.UserId ?? Guid.Empty;

            // Acknowledge the receipt of the message
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            
            // Prepare the response message
            var response = Encoding.UTF8.GetBytes(userId.ToString());
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;

            // Send the response back to the reply queue
            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: props, body: response);
        };

        // Start consuming from the queue
        _channel.BasicConsume(queue: _queueRequest, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        _channel?.Dispose();  // Освобождение канала
        _connection?.Dispose();  // Освобождение соединения
        base.Dispose();
    }
}
