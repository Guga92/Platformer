using Services.Health;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Views
{
    public class DamageView : MonoBehaviour
    {
        private IStatsService _stats;
        
        [Inject]
        public void Construct(IStatsService stats)
        {
            _stats = stats;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) 
                return;
            
            _stats.TakeDamage(1000000);
        }
    }
}