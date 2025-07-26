using System.Collections;
using UnityEngine.SceneManagement;

namespace Services.SceneLoading
{
    public class SceneLoadingIndexStage : ILoadingStage
    {
        private readonly int _index;

        public SceneLoadingIndexStage(int index)
        {
            _index = index;
        }

        public IEnumerator ExecuteAsync()
        {
            yield return SceneManager.LoadSceneAsync(_index);
        }
    }
}