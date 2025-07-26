using System.ComponentModel;
using DefaultNamespace.Views;
using Services.SceneLoading;
using Static;
using Zenject;

namespace DefaultNamespace.Installers
{
    public class LoadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Resolve<ILoadingPipelineService>()
                .LoadGameScene(Scenes.Level_1);
        }
    }
}