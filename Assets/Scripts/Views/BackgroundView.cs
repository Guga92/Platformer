using UnityEngine;

namespace DefaultNamespace.Views
{
    public class BackgroundView : MonoBehaviour
    {
        [field: SerializeField] public Vector2 ParallaxMultiplier { get; private set; } = Vector2.one;
        [field: SerializeField] public float ExtraWidth { get; private set; } = 50f;
        
        public SpriteRenderer Renderer { get; private set; }

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }
    }
}