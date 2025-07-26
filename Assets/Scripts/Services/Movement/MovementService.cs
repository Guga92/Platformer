using DefaultNamespace.Services.GameLoop;
using DefaultNamespace.Services.Input;
using UniRx;
using UnityEngine;
using Zenject;
using CompositeDisposable = UniRx.CompositeDisposable;

namespace DefaultNamespace.Services.Movement
{
    public class MovementService : IMovementService, IInitializable, IEveryUpdate
    {
        public bool IsGrounded { get; private set; }

        private readonly IInputService _input;
        private readonly PlayerView _view;

        private float _currentSpeed;
        private float _verticalVelocity;

        private readonly CompositeDisposable _disposable = new();

        public MovementService(IInputService input, PlayerView view)
        {
            _input = input;
            _view = view;
        }

        public void Initialize()
        {
            _input.JumpCommand
                .Where(_ => IsGrounded)
                .Subscribe(_ => _verticalVelocity = _view.JumpHeight)
                .AddTo(_disposable);
        }

        public void Update()
        {
            if (_view == null || _view.gameObject == null) 
                return;
            
            UpdateInput();
            ApplyGravity();
            
            var move = ComputeMovement();
            move = ApplyCollision(move);
            
            MoveCharacter(move);
            UpdateAnimator();
        }

        private void UpdateInput()
        {
            var control = IsGrounded ? 1f : _view.AirControl;
            _currentSpeed = Mathf.Lerp(_currentSpeed, _input.Direction * _view.Speed, control);
        }

        private void ApplyGravity()
        {
            _verticalVelocity += _view.Gravity * Time.deltaTime;
        }

        private Vector2 ComputeMovement()
        {
            return new Vector2(_currentSpeed * Time.deltaTime, _verticalVelocity * Time.deltaTime);
        }

        private Vector2 ApplyCollision(Vector2 move)
        {
            var origin = _view.GroundChecker.position;
            var size = _view.CheckSize;
            var layer = _view.GroundLayer;

            if (move.x != 0)
            {
                var dir = new Vector2(Mathf.Sign(move.x), 0);
                var dist = Mathf.Abs(move.x) + 0.01f;
                var hit = Physics2D.BoxCast(origin, size, 0f, dir, dist, layer);

                if (hit.collider != null)
                {
                    float adjusted = hit.distance - 0.01f;
                    move.x = Mathf.Sign(move.x) * Mathf.Min(adjusted, Mathf.Abs(move.x));
                    _currentSpeed = 0f;
                }
            }

            if (move.y != 0)
            {
                var dir = new Vector2(0, Mathf.Sign(move.y));
                var dist = Mathf.Abs(move.y) + 0.01f;
                var hit = Physics2D.BoxCast(origin, size, 0f, dir, dist, layer);

                if (hit.collider != null)
                {
                    float adjusted = hit.distance - 0.01f;
                    move.y = Mathf.Sign(move.y) * Mathf.Min(adjusted, Mathf.Abs(move.y));

                    if (move.y < 0)
                    {
                        IsGrounded = true;
                        _verticalVelocity = 0f;
                    }
                    else
                        _verticalVelocity = 0f;
                }
                else
                    IsGrounded = false;
            }
            else
                CheckGround();

            return move;
        }

        private void CheckGround()
        {
            var origin = _view.GroundChecker.position;
            var size = _view.CheckSize;
            IsGrounded = Physics2D.OverlapBox(origin, size, 0f, _view.GroundLayer);
        }

        private void MoveCharacter(Vector2 move)
        {
            _view.transform.position += (Vector3)move;

            if (_input.Direction != 0)
                _view.transform.localScale = new Vector3(Mathf.Sign(_input.Direction), 1, 1);
        }

        private void UpdateAnimator()
        {
            _view.Animator.SetBool("ItsMove", Mathf.Abs(_currentSpeed) > 1f && IsGrounded);
            _view.Animator.SetBool("Fall", !IsGrounded);
        }
    }
}
