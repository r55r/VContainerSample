using UnityEngine.SceneManagement;

namespace Service
{
    public sealed class SceneLoader
    {
        public void LoadSampleScene() => SceneManager.LoadScene(1);
    }
}