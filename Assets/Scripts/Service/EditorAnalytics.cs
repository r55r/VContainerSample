using UnityEngine;

namespace Service
{
    public sealed class EditorAnalytics : IAnalyticsService
    {
        public void SendEvent(string name) =>
            Debug.Log($"EditorAnalytics.SendEvent: '{name}'");
    }
}