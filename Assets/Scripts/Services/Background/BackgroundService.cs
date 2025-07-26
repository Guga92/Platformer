using DefaultNamespace.Services.GameLoop;
using DefaultNamespace.Views;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Services.Background
{
    public class BackgroundService : IBackgroundService, IInitializable, IEveryLateUpdate
    {
        private readonly BackgroundView _background;
        private readonly CameraView _camera;
        
        private Vector3 _lastCameraPosition;
        private float _baseTextureWidth;

        public BackgroundService
        (
            BackgroundView background,
            CameraView camera
        )
        {
            _background = background;
            _camera = camera;
        }

        public void Initialize()
        {
            if(_background == null || _camera == null)
                return;
            
            if(_background.gameObject == null || _camera.gameObject == null)
                return;

            _baseTextureWidth = _background.Renderer.sprite.texture.width / _background.Renderer.sprite.pixelsPerUnit;
            _lastCameraPosition = _camera.transform.position;
            
            UpdateTiling(true);
        }

        public void LateUpdate()
        {
            if(_background == null || _camera == null)
                return;
            
            if(_background.gameObject == null || _camera.gameObject == null)
                return;


            var deltaMovement = _camera.transform.position - _lastCameraPosition;
        
            _background.transform.position += new Vector3(deltaMovement.x * _background.ParallaxMultiplier.x, deltaMovement.y * _background.ParallaxMultiplier.y, 0f);
            _lastCameraPosition = _camera.transform.position;
            
            UpdateTiling();
        }

        private void UpdateTiling(bool force = false)
        {
            var camHalfWidth = _camera.Camera.orthographicSize * _camera.Camera.aspect;
            var camLeft = _camera.transform.position.x - camHalfWidth;
            var camRight = _camera.transform.position.x + camHalfWidth;

            var bgLeft = _background.transform.position.x - _background.Renderer.size.x * 0.5f;
            var bgRight = _background.transform.position.x + _background.Renderer.size.x * 0.5f;

            if (!force && !(camLeft < bgLeft) && !(camRight > bgRight)) 
                return;
            
            var targetWidth = (camRight - camLeft) + _background.ExtraWidth;
            var repeatCount = Mathf.Ceil(targetWidth / _baseTextureWidth);
            
            _background.Renderer.size = new Vector2(repeatCount * _baseTextureWidth, _background.Renderer.size.y);
        }
    }
}