using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sisgea.GerarHorario.Core;

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

  // using var channel = connection.CreateModel();
  // channel.QueueDeclare(queue: "pedido_gerar_horario",
  //                      durable: true,
  //                      exclusive: false,
  //                      autoDelete: false,
  //                      arguments: null);

  // Console.WriteLine(" [*] Waiting for messages.");

  // var consumer = new EventingBasicConsumer(channel);
  // consumer.Received += (model, ea) =>
  // {
  //     var body = ea.Body.ToArray();
  //     var message = Encoding.UTF8.GetString(body);
  //     Console.WriteLine($" [x] Received {message}");
  // };

  // channel.BasicConsume(queue: "hello",
  //                      autoAck: true,
  //                      consumer: consumer);

  Console.WriteLine(" Press [enter] to exit.");
  Console.ReadLine();
});

threadApi.Start();
threadFila.Start();
