using System.Collections;
using DefaultNamespace.Views;
using DG.Tweening;

namespace Services.SceneLoading
{
    public class ShowLoadingAnimationStage : ILoadingStage
    {
        private readonly LoadingView _view;

        public ShowLoadingAnimationStage(LoadingView view)
        {
            _view = view;
        }

        public IEnumerator ExecuteAsync()
        {
            _view.gameObject.SetActive(true);
            _view.Fade.alpha = 0;

            var tween = _view.Fade.DOFade(0.7f, 0.4f);
            yield return tween.WaitForCompletion();
        }
    }
}