using Confluent.Kafka;
using ConsumerMessageUser.Entities;
using ConsumerMessageUser.Model;

namespace ConsumerMessageUser;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _topic;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _configuration = configuration; 
        _serviceScopeFactory = serviceScopeFactory; 

        // Configuração do consumidor Kafka
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["KAFKA_BOOTSTRAP_SERVERS"], // Servidores Kafka
            GroupId = configuration["KAFKA_GROUP_CONSUMIDOR"], // ID do grupo de consumidores
            AutoOffsetReset = AutoOffsetReset.Earliest // Configuração de reset de offset para o mais antigo
        };

        // Criação do consumidor Kafka
        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        // Tópico Kafka a ser consumido
        _topic = configuration["KAFKA_TOPIC"];
    }

    // Método que é executado quando o serviço é iniciado
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Inscreve o consumidor no tópico Kafka
        _consumer.Subscribe(_topic);

        try
        {
            // Loop para consumir mensagens enquanto o serviço não for cancelado
            while (!stoppingToken.IsCancellationRequested)
            {
                // Consome uma mensagem do Kafka
                var consumeResult = _consumer.Consume(stoppingToken);
                // Desserializa a mensagem JSON para um objeto User
                User user = System.Text.Json.JsonSerializer.Deserialize<User>(consumeResult.Message.Value);

                // Insere o usuário no banco de dados usando UserModel
                int insetido = await (new UserModel(_serviceScopeFactory)).Incluir(user);

                // Registra uma mensagem de informação no log
                _logger.LogInformation($"Mensagem recebida: {consumeResult.Message.Value} - {insetido}");
            }
        }
        catch (OperationCanceledException)
        {
            // Fecha o consumidor de forma graciosa quando a operação é cancelada
            _consumer.Close();
        }

        await Task.CompletedTask;
    }
}
