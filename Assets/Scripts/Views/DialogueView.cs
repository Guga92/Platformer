using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public Image Avatar { get; private set; }
    [field: SerializeField] public TextMeshProUGUI NameField { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextField { get; private set; }
    
    [field: Header("Service")]
    [field: SerializeField] public CanvasGroup Fade { get; private set; }
    [field: SerializeField] public Button Exit { get; private set; }

}
