using Services.Dialogue;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace DefaultNamespace.Views
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Sprite _avatar;
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _text;
        
        [SerializeField] private UnityEvent _onDialogueEnd;
        
        private IDialogueService _service;
        private bool _isTriggered;

        [Inject]
        public void Construct(IDialogueService service)
        {
            _service = service;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_isTriggered || other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;
            
            _service.SetDialogue(_avatar, _name, _text);
            _service.SetActive(true);

            _service.OnStateChange
                .Where(b => !b)
                .Take(1)
                .Subscribe(_ => _onDialogueEnd.Invoke());
            
            _isTriggered = true;
        }
    }
}