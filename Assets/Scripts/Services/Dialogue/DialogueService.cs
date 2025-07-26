using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services.Dialogue
{
    public class DialogueService : IDialogueService, IInitializable, IDisposable
    {
        public bool DialogueActive { get; private set; }
        public ReactiveCommand<bool> OnStateChange { get; } = new();

        private readonly DialogueView _view;
        private readonly CompositeDisposable _disposable = new();
        
        public DialogueService
        (
            DialogueView view
        )
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.Exit.onClick
                .AsObservable()
                .Subscribe(_ => SetActive(false))
                .AddTo(_disposable);
        }

        public void SetDialogue(Sprite Avatar, string name, string text)
        {
            _view.Avatar.sprite = Avatar;
            _view.NameField.text = name;
            _view.TextField.text = text;
        }

        public void SetActive(bool active)
        {
            _view.Fade.alpha = active ? 0 : 1f;
            
            _view.Fade
                .DOFade(active ? 1 : 0, 0.5f)
                .OnComplete(() => _view.gameObject.SetActive(active));
            
            DialogueActive = active;
            
            OnStateChange?.Execute(active);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}