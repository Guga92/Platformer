using DG.Tweening;
using Services.Health;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Views
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class HealView : MonoBehaviour
    {
        private IStatsService _stats;
        
        private Tween _moveTween;
        private Tween _rotateTween;

        [Inject]
        public void Construct(IStatsService stats)
        {
            _stats = stats;
        }

        private void Start()
        {
            _moveTween = transform.DOMoveY(transform.position.y + 0.3f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            
            _rotateTween = transform.DORotate(new Vector3(0, 360f, 0), 2f, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) 
                return;
            
            _moveTween?.Kill();
            _rotateTween?.Kill();

            Destroy(gameObject);
            _stats.Heal(1);
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
            _rotateTween?.Kill();
        }
    }
}