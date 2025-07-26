using System;
using System.Collections;
using CoroutineRunner.Interface;
using DefaultNamespace.Services.GameLoop;
using Services.Health;
using Services.Pool;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Views
{
    public class ArrowView : MonoBehaviour, IEveryFixedUpdate
    {
        [SerializeField] private LayerMask _layerMask;
        
        private ArrowPoolService _pool;
        private IStatsService _statsService;
        
        private Rigidbody2D _rigidbody2D;
        
        private Coroutine _autoDestroyCoroutine;
        
        private bool _isActive;

        [Inject]
        public void Construct(ArrowPoolService pool, IStatsService stats)
        {
            _pool = pool;
            _statsService = stats;
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 velocity)
        {
            _isActive = true;

            if (_autoDestroyCoroutine != null)
            {
                StopCoroutine(_autoDestroyCoroutine);
                _autoDestroyCoroutine = null;
            }

            _autoDestroyCoroutine = StartCoroutine(AutoDestroy());

            GetComponent<Rigidbody2D>().velocity = velocity;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isActive)
                return;

            if ((_layerMask.value & (1 << other.gameObject.layer)) == 0)
                return;
            
            if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
                _statsService.TakeDamage(1);
            
            Inactive();
        }
        
        public void FixedUpdate()
        {
            if (!_isActive || _rigidbody2D.velocity.sqrMagnitude < 0.01f)
                return;

            var velocity = _rigidbody2D.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            
            if (transform.localScale.x < 0)
                angle += 180f;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private IEnumerator AutoDestroy()
        {
            yield return new WaitForSeconds(5f);
            
            if (_isActive)
                Inactive();
        }
        
        private void Inactive()
        {
            _isActive = false;

            if (_autoDestroyCoroutine != null)
            {
                StopCoroutine(_autoDestroyCoroutine);
                _autoDestroyCoroutine = null;
            }

            _pool.Despawn(this);
        }
    }
}