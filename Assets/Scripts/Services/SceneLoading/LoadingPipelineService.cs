using System.Collections;
using System.Collections.Generic;
using CoroutineRunner.Interface;
using DefaultNamespace.Views;

namespace Services.SceneLoading
{
    public class LoadingPipelineService : ILoadingPipelineService
    {
        private readonly ICoroutineRunner _runner;
        private readonly LoadingView _view;

        public LoadingPipelineService
        (
            ICoroutineRunner runner,
            LoadingView view
        )
        {
            _runner = runner;
            _view = view;
        }
        
        public void LoadGameScene(string name)
        {
            var stages = new List<ILoadingStage>()
            {
                new ShowLoadingAnimationStage(_view),
                new SceneLoadingNameStage(name),
                new HideLoadingAnimationStage(_view)
            };

            _runner.StartCoroutine(StartLoadingAsync(stages));
        }

        public void LoadGameScene(int index)
        {
            var stages = new List<ILoadingStage>()
            {
                new ShowLoadingAnimationStage(_view),
                new SceneLoadingIndexStage(index),
                new HideLoadingAnimationStage(_view)
            };

            _runner.StartCoroutine(StartLoadingAsync(stages));
        }

        private IEnumerator StartLoadingAsync(List<ILoadingStage> stages)
        {
            foreach (var stage in stages)
                yield return stage.ExecuteAsync();
        }
    }
}