using Prometheus;

namespace HCM.Shared.Metrics;

public static class MetricsRegistry
{
    public static readonly Counter LoginsCounter = Prometheus.Metrics.CreateCounter("app_logins_total", "Total number of successful logins");
}
