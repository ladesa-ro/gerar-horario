using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sisgea.GerarHorario.Core;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

var threadApi = new Thread(() =>
{
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.MapGet("/", () => "Hello World!");

    app.Run();
});


var threadFila = new Thread(() =>
{
    var factory = new ConnectionFactory { HostName = "localhost" };
    factory.UserName = "admin";
    factory.Password = "admin";
    using var connection = factory.CreateConnection();

    using var channel = connection.CreateModel();
    channel.QueueDeclare(
        queue: "gerar_horario",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null
    );

    channel.QueueDeclare(
        queue: "horario_gerado",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null
    );

    void publicarRespostaGerarHorario(HorarioGerado? horarioGerado = null)
    {
        // TODO: message = JSON(horarioGerado)
        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "horario_gerado",
                             basicProperties: null,
                             body: body);

        Console.WriteLine($" [x] Sent {message}");
    }

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] Received {message}");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        GerarHorarioOptions? gerarHorarioOptions = JsonSerializer.Deserialize<GerarHorarioOptions>(message, options);
        Console.WriteLine(gerarHorarioOptions);

        publicarRespostaGerarHorario();
    };

    channel.BasicConsume(queue: "gerar_horario",
                         autoAck: true,
                         consumer: consumer);

    Console.ReadLine();
});

threadFila.Start();
threadApi.Start();
