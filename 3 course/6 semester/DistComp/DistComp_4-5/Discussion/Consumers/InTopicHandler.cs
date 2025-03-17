using Discussion.DTO.Request;
using Messaging;
using Messaging.Consumer.Interfaces;

namespace Discussion.Consumers;

public class InTopicHandler : IKafkaHandler<string, KafkaMessage<NoticeRequestDTO>>
{
    public async Task HandleAsync(string key, KafkaMessage<NoticeRequestDTO> value)
    {
    }
}