using System.Diagnostics;
using MeteoService.API.Infrastructure.HealthChecks;

namespace MeteoService.API.API.Middlewares;

public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            PerformanceHealthCheck.RecordResponseTime(stopwatch.ElapsedMilliseconds);
        }
    }
}