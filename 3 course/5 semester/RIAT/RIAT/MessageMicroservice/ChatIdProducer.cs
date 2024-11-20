using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageMicroservice;

public class ChatIdProducer : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _requestQueue;
    private readonly string _replyQueue;
    
    public ChatIdProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        _factory = _configuration.GetSection("RabbitMQ").Get<ConnectionFactory>()!;
        _requestQueue = _configuration["RabbitMQ:RequestQueueChatId"]!;
        _replyQueue = _configuration["RabbitMQ:ResponseQueueChatId"]!;

        // Создание соединения и канала RabbitMQ
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Объявление очередей
        _channel.QueueDeclare(queue: _requestQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: _replyQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }
    
    // This method requests the ChatId by ChatName from the User Microservice
    public async Task<Guid?> GetChatIdByChatNameAsync(string chatName)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        var correlationId = Guid.NewGuid().ToString();
        var props = channel.CreateBasicProperties();
        
        // Set ReplyTo and CorrelationId so the consumer can respond correctly
        props.ReplyTo = _replyQueue;
        props.CorrelationId = correlationId;

        // Serialize the request message to send to the User Microservice
        var message = new { ChatName = chatName };
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        // Publish the request to the queue
        channel.BasicPublish(exchange: "", routingKey: _requestQueue, basicProperties: props, body: body);

        var tcs = new TaskCompletionSource<Guid?>();

        // Create a consumer to listen for the response from the User Microservice
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            // Match the response using CorrelationId
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                if (Guid.TryParse(response, out var chatId))
                {
                    tcs.SetResult(chatId);
                }
                else
                {
                    tcs.SetResult(null);
                }
            }
        };

        // Start consuming from the reply queue
        channel.BasicConsume(queue: _replyQueue, autoAck: true, consumer: consumer);

        // Wait for the result
        return await tcs.Task;
    }
    
    public void Dispose()
    {
        _channel?.Close();  // Закрытие канала
        _connection?.Close();  // Закрытие соединения
        _channel?.Dispose();  // Освобождение канала
        _connection?.Dispose();  // Освобождение соединения
    }
}