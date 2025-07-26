using UniRx;

namespace DefaultNamespace.Services.Input
{
    public interface IInputService
    {
        public float Direction { get; }
        
        public ReactiveCommand JumpCommand { get; }
        
        public ReactiveCommand AttackCommand { get; }
    }
}