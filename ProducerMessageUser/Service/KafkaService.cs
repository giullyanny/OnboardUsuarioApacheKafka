using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace ProducerMessageUser.Service
{
    public class KafkaService
    {
        private readonly IProducer<string, string> _producer;

        public KafkaService(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                // Endereço do servidor Kafka
                BootstrapServers = configuration["KAFKA_BOOTSTRAP_SERVERS"] ?? "gokafka_kafka_1:9092",
                // Identificação do cliente Kafka
                ClientId = configuration["KAFKA_CLIENT_ID"],
                // Confirmação de entrega para todas as réplicas
                Acks = Acks.All,
                // Ativa a idempotência para evitar duplicação de mensagens
                EnableIdempotence = true
            };

            // Cria o produtor Kafka com as configurações fornecidas
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic, string message, string? key = null)
        {
            try
            {
                string keyKafka = (key ?? string.Empty);

                // Produz a mensagem no tópico especificado
                var result = await _producer.ProduceAsync(topic, new Message<string, string> { Key = keyKafka, Value = message });

                Console.WriteLine($"Mensagem produzida para o tópico {topic} [Partition: {result.Partition}, Offset: {result.Offset}]");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao produzir mensagem: {e.Message}");
            }
        }
    }
}
