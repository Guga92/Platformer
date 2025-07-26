using UnityEngine;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    [field: SerializeField] public Image[] HealthItems { get; private set; }
    [field: SerializeField] public Image[] ManaItems { get; private set; }
}
