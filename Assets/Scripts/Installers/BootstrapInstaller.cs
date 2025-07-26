using CoroutineRunner.Interface;
using DefaultNamespace.Views;
using Services.SceneLoading;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LoadingView _view;
        
        public override void InstallBindings()
        {
            var loader = Instantiate(_view);
            var runner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner.CoroutineRunner>();
            
            loader.gameObject.SetActive(false);
            
            DontDestroyOnLoad(runner);
            DontDestroyOnLoad(loader);
            
            Container
                .Bind<ICoroutineRunner>()
                .FromInstance(runner)
                .AsSingle();
            
            Container
                .Bind<LoadingView>()
                .FromInstance(loader)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<LoadingPipelineService>()
                .AsSingle();
        }
    }
}