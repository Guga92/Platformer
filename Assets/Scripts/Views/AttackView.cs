using UnityEngine;

namespace DefaultNamespace.Views
{
    public class AttackView : MonoBehaviour
    {
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public Vector2 Size { get; private set; }
        
        [field: SerializeField] public Transform Origin { get; private set; }
        [field: SerializeField] public LayerMask Layer { get; private set; }
        
        [field: SerializeField] public AudioClip[] DamageSounds { get; private set; }
        
        private void OnDrawGizmos()
        {
            if (Origin is null)
                return;

            var direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            var origin = Origin.position;
            var castCenter = origin + (Vector3)direction * (Range * 0.5f);
    
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(castCenter, Origin.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, Size + new Vector2(Range, 0));
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}