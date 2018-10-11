namespace Cactus.Aspnetcore.MetricMiddleware
{
    public class MetricMiddlewareConfig
    {
        /// <summary>
        /// Buckets upper levels (milliseconds). Affect performance (fewer - better performance) Default: {50.0, 100, 200, 500, 1000, 1500, 2000}
        /// </summary>
        public double[] ProceedRequestTimeBuckets { get; set; }

        /// <summary>
        /// Exclude getting status code metric. DEFAULT: false
        /// </summary>
        public bool ExcludeStatusCodeMetric { get; set; }

        public MetricMiddlewareConfig()
        {
            ProceedRequestTimeBuckets = new[] {50.0, 100, 200, 500, 1000, 1500, 2000};

            ExcludeStatusCodeMetric = false;
        }
    }
}
