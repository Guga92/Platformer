using System.Collections;
using CoroutineRunner.Interface;
using DG.Tweening;
using Services.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Services.Health
{
    public class StatsService : IStatsService, IInitializable
    {
        public int CurrentHealth { get; private set; }
        public int CurrentMana { get; private set; }
        
        private readonly GameUIView _ui;
        private readonly PlayerView _player;
        private readonly ILoadingPipelineService _loading;
        private readonly ICoroutineRunner _runner;

        private const int _maxHealth = 4;
        private const int _maxMana = 4;
        
        public StatsService
        (
            GameUIView ui,
            PlayerView player,
            ILoadingPipelineService loading,
            ICoroutineRunner runner
        )
        {
            _ui = ui;
            _player = player;
            _loading = loading;
            _runner = runner;
        }

        public void Initialize()
        {
            CurrentHealth = _maxHealth;
            CurrentMana = _maxMana;
            
            UpdateUI();
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            
            AudioSource.PlayClipAtPoint(_player.DamageSound, _player.transform.position);
            var render = _player.GetComponent<SpriteRenderer>();
            
            if(CurrentHealth <= 0)
                Die();
            else
                render.DOColor(Color.red, 0.3f)
                    .OnComplete(() => render.DOColor(Color.white, 0.3f));
            
            UpdateUI();
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            
            AudioSource.PlayClipAtPoint(_player.HealSound, _player.transform.position);
            
            if(CurrentHealth >= _maxHealth)
                CurrentHealth = _maxHealth;
            
            UpdateUI();
        }

        public void UseMana(int amount)
        {
            CurrentMana -= amount;

            if (CurrentMana <= 0)
                CurrentMana = 0;
            
            UpdateUI();
        }

        public void TakeMana(int amount)
        {
            CurrentMana += amount;
            
            if(CurrentMana >= _maxMana) 
                CurrentMana = _maxMana;
            
            UpdateUI();
        }

        private void Die()
        {
            if(_player == null || _player.gameObject == null)
                return;
            
            var components = _player.GetComponents<MonoBehaviour>();

            foreach (var component in components)
                Object.Destroy(component);
            
            _player.Animator.SetTrigger("ItsDeath");
            
            var render = _player.GetComponent<SpriteRenderer>();
            
            render.DOColor(Color.red, 0.5f)
                .OnComplete(() => render.DOFade(0, 0.5f));
            
            _runner.StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel()
        {
            yield return new WaitForSeconds(2);
            
            _loading.LoadGameScene(SceneManager.GetActiveScene().name);
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _ui.HealthItems.Length; i++)
                _ui.HealthItems[i].gameObject.SetActive(i < CurrentHealth);
            
            for (int i = 0; i < _ui.ManaItems.Length; i++)
                _ui.ManaItems[i].gameObject.SetActive(i < CurrentMana);
        }
    }
}