using System.Collections;
using DefaultNamespace.Views;
using DG.Tweening;

namespace Services.SceneLoading
{
    public class HideLoadingAnimationStage : ILoadingStage
    {
        private readonly LoadingView _view;

        public HideLoadingAnimationStage(LoadingView view)
        {
            _view = view;
        }
        
        public IEnumerator ExecuteAsync()
        {
            var tween = _view.Fade
                .DOFade(0, 0.7f)
                .OnComplete(() => _view.gameObject.SetActive(false));
            
            yield return tween.WaitForCompletion();
        }
    }
}