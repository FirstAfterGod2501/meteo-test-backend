using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeteoService.API.Infrastructure.HealthChecks
{
    public class PerformanceHealthCheck : IHealthCheck
    {
        private static long _requestCount = 0;
        private static long _totalResponseTime = 0;

        public static void RecordResponseTime(long responseTime)
        {
            Interlocked.Increment(ref _requestCount);
            Interlocked.Add(ref _totalResponseTime, responseTime);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var averageResponseTime = (_requestCount > 0) ? _totalResponseTime / _requestCount : 0;
            var isHealthy = averageResponseTime < 1000; // Example threshold

            return Task.FromResult(isHealthy
                ? HealthCheckResult.Healthy($"Average response time: {averageResponseTime}ms")
                : HealthCheckResult.Unhealthy($"High average response time: {averageResponseTime}ms"));
        }
    }
}