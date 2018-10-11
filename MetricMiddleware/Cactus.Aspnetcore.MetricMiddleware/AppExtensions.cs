using Microsoft.AspNetCore.Builder;

namespace Cactus.Aspnetcore.MetricMiddleware
{
    public static class AppExtensions
    {
        /// <summary>
        /// Gets response time and status codes count (OPTIONAL) for every request.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCactusMetric(this IApplicationBuilder app, MetricMiddlewareConfig config = null)
        {
            if (config == null)
                config = new MetricMiddlewareConfig();

            app.UseMiddleware<MetricMiddleware>(config);
            return app;
        }
    }
}
