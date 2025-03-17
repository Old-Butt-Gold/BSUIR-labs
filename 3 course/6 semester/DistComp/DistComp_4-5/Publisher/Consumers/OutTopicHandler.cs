using Messaging;
using Messaging.Consumer.Interfaces;
using Publisher.DTO.ResponseDTO;

namespace Publisher.Consumers;

public class OutTopicHandler : IKafkaHandler<string, KafkaMessage<NoticeResponseDTO>>
{
    public async Task HandleAsync(string key, KafkaMessage<NoticeResponseDTO> value)
    {
    }
}