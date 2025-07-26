using CoroutineRunner.Interface;
using DefaultNamespace.Services.Attack;
using DefaultNamespace.Services.Background;
using DefaultNamespace.Services.Camera;
using DefaultNamespace.Services.GameLoop;
using DefaultNamespace.Services.Input;
using DefaultNamespace.Services.Movement;
using DefaultNamespace.Views;
using Services.Dialogue;
using Services.EnemyLOD;
using Services.Health;
using Services.Pool;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _arrowPrefab;
        
        public override void InstallBindings()
        {
            Container.BindMemoryPool<ArrowView, ArrowPoolService>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_arrowPrefab)
                .UnderTransformGroup("ArrowPool");

            Bind<DialogueService>();
            Bind<InputService>();

            Bind<BackgroundService>();
            Bind<CameraService>();
            Bind<MovementService>();
            Bind<AttackService>();
            Bind<StatsService>();
            Bind<EnemyLODService>();

            Bind<GameLoopService>();
        }
        
        private void Bind<T>() where T : class
        {
            Container
                .BindInterfacesAndSelfTo<T>()
                .AsSingle();
        }
    }
}