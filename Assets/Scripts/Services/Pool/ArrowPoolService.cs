using DefaultNamespace.Views;
using Zenject;

namespace Services.Pool
{
    public class ArrowPoolService : MonoMemoryPool<ArrowView>
    {
        protected override void OnDespawned(ArrowView item)
        {
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(ArrowView item)
        {
            item.gameObject.SetActive(true);
        }
    }
}