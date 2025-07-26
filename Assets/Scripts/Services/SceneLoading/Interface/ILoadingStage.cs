using System.Collections;

namespace Services.SceneLoading
{
    public interface ILoadingStage
    {
        public IEnumerator ExecuteAsync();
    }
}