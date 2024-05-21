using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;
using Sisgea.GerarHorario.Core;


#region Queue Tweaks

var factory = new ConnectionFactory()
{
    HostName = Environment.GetEnvironmentVariable("HostName"),
    UserName = Environment.GetEnvironmentVariable("UserName"),
    Password = Environment.GetEnvironmentVariable("Password")
};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();
channel.QueueDeclare(
    queue: Environment.GetEnvironmentVariable("ListenQueue"),
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

channel.QueueDeclare(
    queue: Environment.GetEnvironmentVariable("PublishQueue"),
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

#endregion


Console.WriteLine(" [*] Waiting for messages.");

var serializationOptions = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
};

var consumer = new EventingBasicConsumer(channel);
consumer.Received += ListenResponseInGerarHorario;

channel.BasicConsume(queue: Environment.GetEnvironmentVariable("ListenQueue"),
    autoAck: true,
    consumer: consumer);


#region Gerar Horario

IEnumerable<HorarioGerado> GerarHorario(GerarHorarioOptions options)
{
    return Gerador.GerarHorario(options);
}

void ListenResponseInGerarHorario(object? model, BasicDeliverEventArgs ea)
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);


    Console.WriteLine($" [x] Received {message}");


    GerarHorarioOptions? gerarHorarioOptions =
        JsonSerializer.Deserialize<GerarHorarioOptions>(message, serializationOptions);


    var horarioGerado = GerarHorario(gerarHorarioOptions);

    var horarioJson = JsonSerializer.Serialize(horarioGerado);

    PublishResponseIntoHorarioGerado(horarioJson);
}

void PublishResponseIntoHorarioGerado(string horarioGerado)
{
    var body = Encoding.UTF8.GetBytes(horarioGerado);

    channel.BasicPublish(exchange: string.Empty,
        routingKey: Environment.GetEnvironmentVariable("PublishQueue"),
        basicProperties: null,
        body: body);

    Console.WriteLine($" [x] Sent {horarioGerado}");
}

#endregion