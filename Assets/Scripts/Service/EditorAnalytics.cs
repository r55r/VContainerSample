using JetBrains.Annotations;
using UnityEngine;

namespace Service
{
    [UsedImplicitly]
    public sealed class EditorAnalytics : IAnalyticsService
    {
        public void SendEvent(string name) =>
            Debug.Log($"EditorAnalytics.SendEvent: '{name}'");
    }
}