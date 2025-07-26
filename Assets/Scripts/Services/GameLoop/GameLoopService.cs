using System.Collections.Generic;
using UniRx;
using Zenject;

namespace DefaultNamespace.Services.GameLoop
{
    public class GameLoopService : IInitializable
    {
        private readonly List<IEveryUpdate> _updates;
        private readonly List<IEveryFixedUpdate> _fixedUpdates;
        private readonly List<IEveryLateUpdate> _lateUpdates;

        private readonly CompositeDisposable _disposables = new();

        public GameLoopService
        (
            List<IEveryUpdate> updates,
            List<IEveryFixedUpdate> fixedUpdates,
            List<IEveryLateUpdate> lateUpdates
        )
        {
            _updates = updates;
            _fixedUpdates = fixedUpdates;
            _lateUpdates = lateUpdates;
        }

        public void Initialize()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    foreach (var update in _updates)
                        update.Update();
                })
                .AddTo(_disposables);
            
            Observable
                .EveryFixedUpdate()
                .Subscribe(_ =>
                {
                    foreach (var update in _fixedUpdates)
                        update.FixedUpdate();
                })
                .AddTo(_disposables);
            
            Observable
                .EveryLateUpdate()
                .Subscribe(_ =>
                {
                    foreach (var update in _lateUpdates)
                        update.LateUpdate();
                })
                .AddTo(_disposables);
        }
    }
}