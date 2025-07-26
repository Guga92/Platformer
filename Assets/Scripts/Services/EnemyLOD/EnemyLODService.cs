using System.Collections.Generic;
using DefaultNamespace.Services.GameLoop;
using DefaultNamespace.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services.EnemyLOD
{
    public class EnemyLODService : IInitializable, IEveryUpdate
    {
        private readonly List<EnemyView> _enemies = new();
        
        private readonly CameraView _camera;
        private readonly PlayerView _player;

        public EnemyLODService
        (
            CameraView camera,
            PlayerView player
        )
        {
            _camera = camera;
            _player = player;
        }

        public void Initialize()
        {
            var enemy = Object.FindObjectsOfType<EnemyView>();

            foreach (var view in enemy)
            {
                view.OnDeath
                    .Take(1)
                    .Subscribe(_ => _enemies.Remove(view));
                
                _enemies.Add(view);
            }
        }
        
        public void Update()
        {
            if(_player == null || _player.gameObject == null)
                return;
            
            var bounds = GetCameraBounds();

            foreach (var enemy in _enemies)
            {
                var isVisible = bounds.Contains(enemy.transform.position);

                enemy.gameObject.SetActive(isVisible);
                enemy.SetTarget(isVisible ? _player.transform : null);
            }
        }
        
        private Bounds GetCameraBounds()
        {
            var camera = _camera.Camera;
            var camHeight = 2f * camera.orthographicSize;
            var camWidth = camHeight * camera.aspect + 1;
            var camCenter = camera.transform.position;

            return new Bounds(camCenter, new Vector3(camWidth, camHeight, 100f));
        }
    }
}