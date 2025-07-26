using UniRx;
using UnityEngine;

namespace Services.Dialogue
{
    public interface IDialogueService
    {
        public bool DialogueActive { get; }
        
        public ReactiveCommand<bool> OnStateChange { get; }
        
        public void SetDialogue(Sprite Avatar, string name, string text);

        public void SetActive(bool active);
    }
}