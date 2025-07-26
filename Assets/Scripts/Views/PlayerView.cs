using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 10f;
    [field: SerializeField] public float JumpHeight { get; private set; }  = 5f;
    [field: SerializeField] public float Gravity { get; private set; }  = -20f;
    [field: SerializeField] public float AirControl { get; private set; }  = 0.5f;

    [field: SerializeField] public LayerMask GroundLayer { get; private set; } 
    [field: SerializeField] public Transform GroundChecker { get; private set; } 
    [field: SerializeField] public Vector2 CheckSize { get; private set; }  = new(0.2f, 0.05f);
    
    [field: SerializeField] public AudioClip DamageSound { get; private set; }
    [field: SerializeField] public AudioClip HealSound { get; private set; }
    
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    
    private void OnDrawGizmos()
    {
        if (GroundChecker is null) 
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundChecker.position, CheckSize);
    }
}
