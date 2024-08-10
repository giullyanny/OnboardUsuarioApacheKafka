using ProducerMessageUser.Service;
using ProducerMessageUser.Entities;
using ProducerMessageUser.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar o KafkaService ao contêiner de serviços
builder.Services.AddSingleton<KafkaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/create-user", async (UserRequestDTO user, KafkaService kafkaService) =>
{
    User userEntity = new User(user.Name, DateTime.Now, user.Phone, user.Password);
    UserResponseDTO response = new UserResponseDTO(userEntity.Name, userEntity.Create, userEntity.Phone);

    // Serializar o forecast para JSON
    var messageUser = System.Text.Json.JsonSerializer.Serialize(userEntity);

    string topico = builder.Configuration["KAFKA_TOPIC"].ToString();

    // Enviar a mensagem para o Kafka
    await kafkaService.ProduceAsync(topico, messageUser);

    return Results.Ok(new { dados = response, message = "Usuário enviado com sucesso" });
})
.WithOpenApi();

app.Run();
