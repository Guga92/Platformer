using System.Collections;
using CoroutineRunner.Interface;
using DefaultNamespace.Services.Input;
using DefaultNamespace.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Services.Attack
{
    public class AttackService : IInitializable, IAttackService
    {
        public bool IsAttacking { get; private set; }
        
        private readonly IInputService _input;
        private readonly ICoroutineRunner _runner;
        
        private readonly PlayerView _playerView;
        private readonly AttackView _attackView;

        private readonly CompositeDisposable _disposables = new();
        
        public AttackService
        (
            IInputService input,
            ICoroutineRunner runner,
            PlayerView playerView,
            AttackView attackView
        )
        {
            _input = input;
            _runner = runner;
            _playerView = playerView;
            _attackView = attackView;
        }

        public void Initialize()
        {
            _input.AttackCommand
                .Where(_ => !IsAttacking)
                .Subscribe(_ => _runner.StartCoroutine(AttackRoutine()))
                .AddTo(_disposables);
        }
        
        private IEnumerator AttackRoutine()
        {
            IsAttacking = true;
            
            PerformAttack();

            yield return new WaitForSeconds(1);

            IsAttacking = false;
        }

        private void PerformAttack()
        {
            if (_playerView == null || _playerView.gameObject == null)
                return;
            
            _playerView.Animator.SetTrigger("Attack");
            _playerView.Animator.SetInteger("AttackType", Random.Range(1, 4));
            
            var direction = _playerView.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            var origin = _attackView.Origin.position;

            var hits = Physics2D.BoxCastAll(origin, _attackView.Size, 0f, direction, _attackView.Range, _attackView.Layer);

            if(hits.Length == 0)
                return;

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<EnemyView>(out var view))
                {
                    view?.TakeDamage();
                    AudioSource.PlayClipAtPoint(_attackView.DamageSounds[Random.Range(0, _attackView.DamageSounds.Length)], _attackView.transform.position);
                }
            }
        }
    }
}