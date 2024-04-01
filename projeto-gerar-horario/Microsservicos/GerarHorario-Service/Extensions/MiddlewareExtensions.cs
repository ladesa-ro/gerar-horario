using GerarHorario_Service.Middlewares;

namespace GerarHorario_Service.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseQueueListener(this IApplicationBuilder app)
    {
        return app.UseMiddleware<QueueListenerMiddleware>();
    }
}