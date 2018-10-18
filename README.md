# Cactus.Metrics
Middleware for collecting metrics:
1. Proceed request time
1. Response status code counts

Based on [Prometheus](https://prometheus.io) and [Grafana](https://grafana.com/) 

Repository contains a project for middleware and tools script

## How to use
1. Install [prometheus-net.AspNetCore](https://github.com/prometheus-net/prometheus-net). It is needed to expose metric endpoint
1. Install [NuGet package](https://www.nuget.org/packages/Cactus.Aspnetcore.MetricMiddleware/)
1. In Startup.cs add middleware into core pipeline:
  ```c#
  app.UseMetricServer(); // Prometheus exstension to expose metric endpoint
  app.UseCactusMetric();
  ```
4. Clone [ToolsScript](https://github.com/CactusSoft/Cactus.Metrics/tree/master/ToolsScripts) folder to your machine
1. Edit configs to match for your project
1. Use docker-compose.metrics.yml to start tools

## Configuration
### Metric middleware
Use **MetricMiddlewareConfig** object to change standart behavior:
* ExcludeStatusCodeMetric - set true to exclude status code metric
* ProceedRequestTimeBuckets - proceed request time buckets upper levels (milliseconds). Affect performance (fewer - better performance) Default: {50.0, 100, 200, 500, 1000, 1500, 2000} [Details here](https://prometheus.io/docs/concepts/metric_types/#histogram)
### Prometheus
In [ToolScripts/prometheus](https://github.com/CactusSoft/Cactus.Metrics/tree/master/ToolsScripts/prometheus) there are two config files:
* alert.rules - contains rules for Prometheus alerting [Details here](https://prometheus.io/docs/prometheus/latest/configuration/alerting_rules/)
* prometheus.yml - contains Prometheus config [Details here](https://prometheus.io/docs/prometheus/latest/configuration/configuration/)
**Add servers to monitor in scrape_configs section**
### Alertmanager
In [ToolScripts/alertmanager](https://github.com/CactusSoft/Cactus.Metrics/tree/master/ToolsScripts/alertmanager) there is config.yml for alerting configuration [Details here](https://prometheus.io/docs/alerting/configuration/)
### Grafana
In Grafana included two dashboards:
* Docker (actually not perfect without swarm)
* General - shows proceed request times, response status code counts, CPU/memory usage, uptime
You can add your own dashboards in [ToolScriptsgrafana/provisioning/dashboards](https://github.com/CactusSoft/Cactus.Metrics/tree/master/ToolsScripts/grafana/provisioning/dashboards) folder

To update preconfigured dashboards, export it and import back in Grafana ([Details here](http://docs.grafana.org/administration/provisioning/#dashboards))

#### Credentials
For the first login, after that Grafana suggests changing password

Login: admin

Password: admin

**OR** you can set admin password in [ToolsScripts/grafana/config.monitoring](https://github.com/CactusSoft/Cactus.Metrics/blob/master/ToolsScripts/grafana/config.monitoring) file in GF_SECURITY_ADMIN_PASSWORD parameter

### docker-compose.metrics.yml
You should change networks sections and path in volumes sections (if you changed docker-compose.metrics.yml path)
