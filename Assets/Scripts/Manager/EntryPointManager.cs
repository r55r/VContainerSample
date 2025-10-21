using JetBrains.Annotations;
using Service;

namespace Manager
{
    [UsedImplicitly]
    public sealed class EntryPointManager
    {
        readonly SceneLoader _sceneLoader;
        readonly IAnalyticsService _analytics;

        public EntryPointManager(SceneLoader sceneLoader, IAnalyticsService analytics)
        {
            _sceneLoader = sceneLoader;
            _analytics = analytics;
        }

        public void Start()
        {
            _sceneLoader.LoadSampleScene();
            _analytics.SendEvent("Start");
        }
    }
}