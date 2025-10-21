using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace Service
{
    [UsedImplicitly]
    public sealed class SceneLoader
    {
        public void LoadSampleScene() => SceneManager.LoadScene(1);
    }
}