using JetBrains.Annotations;

namespace Service
{
    [UsedImplicitly]
    public sealed class RuntimeAnalytics : IAnalyticsService
    {
        public void SendEvent(string name) { }
    }
}