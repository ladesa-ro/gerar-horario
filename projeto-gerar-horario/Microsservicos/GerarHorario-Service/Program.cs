using GerarHorario_Service.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("up", () => "up");


app.UseQueueListener();

app.Run();