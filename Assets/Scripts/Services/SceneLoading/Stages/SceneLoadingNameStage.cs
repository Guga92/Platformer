using System.Collections;
using UnityEngine.SceneManagement;

namespace Services.SceneLoading
{
    public class SceneLoadingNameStage : ILoadingStage
    {
        private readonly string _sceneName;

        public SceneLoadingNameStage(string sceneName)
        {
            _sceneName = sceneName;
        }

        public IEnumerator ExecuteAsync()
        {
            yield return SceneManager.LoadSceneAsync(_sceneName);
        }
    }
}