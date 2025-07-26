namespace Services.Health
{
    public interface IStatsService
    {
        public int CurrentHealth { get; }
        
        public int CurrentMana { get; }
        
        public void TakeDamage(int amount);
        
        public void Heal(int amount);

        public void UseMana(int amount);
        
        public void TakeMana(int amount);
    }
}