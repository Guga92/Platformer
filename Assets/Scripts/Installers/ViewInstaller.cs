using DefaultNamespace.Views;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace DefaultNamespace.Installers
{
    public class ViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Bind<CameraView>();
            Bind<GameUIView>();
            Bind<PlayerView>();
            Bind<BackgroundView>();
            Bind<AttackView>();
            Bind<DialogueView>();
        }

        private void Bind<T>() where T : MonoBehaviour
        {
            Container
                .Bind<T>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}