using System.Collections;
using CoroutineRunner.Interface;
using Services.Health;
using Services.Pool;
using UniRx;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Views
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Transform _target;
        [SerializeField] private AudioClip _bowSound;
        

        private ArrowPoolService _pool;
        private Coroutine _attackCoroutine;

        private bool _isAlive = true;
        private Animator _animator;

        public readonly ReactiveCommand OnDeath = new();
        public IStatsService _stats;

        [Inject]
        private void Construct(ArrowPoolService pool, IStatsService stats)
        {
            _pool = pool;
            _stats = stats;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_isAlive || _target == null || _stats.CurrentHealth <= 0)
                return;

            var direction = transform.position.x < _target.position.x ? Vector3.right : Vector3.left;
            transform.localScale = new Vector3(direction.x, transform.localScale.y, transform.localScale.z);

            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(AttackRoutine());
        }

        private void OnDisable()
        {
            StopCoroutine(AttackRoutine());
            _attackCoroutine = null;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void TakeDamage()
        {
            if (!_isAlive)
                return;

            _isAlive = false;
            _animator.SetTrigger("Hit");

            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            OnDeath?.Execute();
            
            Destroy(gameObject, 3f);
            Destroy(this, 3f);
        }

        private IEnumerator AttackRoutine()
        {
            while (_isAlive && _stats.CurrentHealth > 0)
            {
                PerformAttack();
                _animator.SetTrigger("Attack");

                yield return new WaitForSeconds(0.7f);
            }

            _attackCoroutine = null;
        }

        private void PerformAttack()
        {
            AudioSource.PlayClipAtPoint(_bowSound, transform.position);
            
            var arrow = _pool.Spawn();
            var direction = transform.position.x < _target.position.x ? Vector3.right : Vector3.left;

            arrow.transform.position = _firePoint.position;
            arrow.transform.localScale = new Vector3(direction.x, arrow.transform.localScale.y, arrow.transform.localScale.z);

            var targetPosition = _target.position;

            if (Random.value < 0.6f)
            {
                var offset = Random.insideUnitCircle.normalized * Random.Range(1f, 3f);
                targetPosition += (Vector3)offset;
            }

            var angle = 45f;
            var gravity = Mathf.Abs(Physics2D.gravity.y);

            var velocity = CalculateVelocityByAngle(_firePoint.position, targetPosition, angle, gravity);

            if (velocity == Vector2.zero)
                velocity = new Vector2(Mathf.Sign(targetPosition.x - _firePoint.position.x), 1f).normalized * 10f;

            arrow.Launch(velocity);
        }

        private Vector2 CalculateVelocityByAngle(Vector2 from, Vector2 to, float angleDegrees, float gravity)
        {
            var diff = to - from;
            var dx = Mathf.Abs(diff.x);
            var dy = diff.y;

            var angleRad = angleDegrees * Mathf.Deg2Rad;

            var cos = Mathf.Cos(angleRad);
            var tan = Mathf.Tan(angleRad);

            var denominator = 2 * (dx * tan - dy);

            if (denominator <= 0.01f)
                return Vector2.zero;

            var vSquared = (gravity * dx * dx) / (denominator * cos * cos);

            if (vSquared < 0)
                return Vector2.zero;

            var speed = Mathf.Sqrt(vSquared);

            var direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            direction.x *= Mathf.Sign(diff.x);

            return direction * speed;
        }
    }
}
