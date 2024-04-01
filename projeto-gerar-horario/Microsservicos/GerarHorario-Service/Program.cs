using GerarHorario_Service.Middlewares;


Thread server = new(() =>
{

    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.MapGet("/up", () => "up");



    app.Run();
});

Thread queue = new(QueueService.ListenQueue);



server.Start();
queue.Start();

