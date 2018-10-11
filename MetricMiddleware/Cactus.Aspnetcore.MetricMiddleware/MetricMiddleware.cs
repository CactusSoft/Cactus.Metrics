using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Prometheus;

namespace Cactus.Aspnetcore.MetricMiddleware
{
    public class MetricMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MetricMiddlewareConfig _config;

        private readonly Stopwatch _stopwatch = new Stopwatch();
        protected readonly Histogram ProceedRequestTimeHistogram;
        protected readonly Counter StatusCodeCounter;

        public MetricMiddleware(RequestDelegate next, MetricMiddlewareConfig config)
        {
            _next = next;
            _config = config;

            ProceedRequestTimeHistogram = Metrics.CreateHistogram("server_proceed_request_time_ms",
                "Time for proceeding request in milliseconds",
                new HistogramConfiguration()
                {
                    LabelNames = new[] {"method", "endpoint"},
                    Buckets = _config.ProceedRequestTimeBuckets
                });

            if (!_config.ExcludeStatusCodeMetric)
            {
                StatusCodeCounter = Metrics.CreateCounter("server_status_code_count", "Server status codes count",
                    "code", "method", "endpoint");
            }
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await GetProceedRequestTimeMetric(httpContext);

            if (!_config.ExcludeStatusCodeMetric)
            {
                GetStatusCodeMetric(httpContext);
            }
        }

        protected virtual async Task GetProceedRequestTimeMetric(HttpContext httpContext)
        {
            try
            {
                _stopwatch.Start();
                await _next(httpContext);
                _stopwatch.Stop();

                ProceedRequestTimeHistogram.WithLabels(httpContext.Request.Method, httpContext.Request.Path)
                    .Observe(_stopwatch.ElapsedMilliseconds);
            }
            finally
            {
                _stopwatch.Reset();
            }
        }

        protected virtual void GetStatusCodeMetric(HttpContext httpContext)
        {
            StatusCodeCounter.WithLabels(
                    httpContext.Response.StatusCode.ToString("D"),
                    httpContext.Request.Method,
                    httpContext.Request.Path)
                .Inc();
        }
    }
}
