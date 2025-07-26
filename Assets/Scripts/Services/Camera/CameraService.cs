using DefaultNamespace.Services.GameLoop;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Services.Camera
{
    public class CameraService : ICameraService, IEveryLateUpdate
    {
        private readonly PlayerView _player;
        private readonly CameraView _camera;

        public CameraService
        (
            CameraView camera,
            PlayerView player
        )
        {
            _player = player;
            _camera = camera;
        }

        public void LateUpdate()
        {
            if (_player == null || _player.gameObject == null) 
                return;
            
            var newPos = Vector3.Lerp(_camera.transform.position, _player.transform.position, _camera.Smoothing * Time.deltaTime);
            var endPos = new Vector3(newPos.x, newPos.y, _camera.transform.position.z);
        
            _camera.transform.position = endPos;
        }
    }
}