using DefaultNamespace.Services.GameLoop;
using Services.Dialogue;
using UniRx;
using UnityEngine;

namespace DefaultNamespace.Services.Input
{
    public class InputService : IInputService, IEveryUpdate
    {
        private readonly IDialogueService _dialogue;
        
        public float Direction { get; private set; }

        public ReactiveCommand JumpCommand { get; private set; } = new();

        public ReactiveCommand AttackCommand { get; private set; } = new();

        public InputService
        (
            IDialogueService dialogue
        )
        {
            _dialogue = dialogue;
        }

        public void Update()
        {
            if (_dialogue.DialogueActive)
            {
                Direction = 0;
                return;
            }
            
            Direction = UnityEngine.Input.GetAxisRaw("Horizontal");
            
            if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
                JumpCommand?.Execute();

            if (UnityEngine.Input.GetMouseButtonDown(0))
                AttackCommand?.Execute();
        }
    }
}