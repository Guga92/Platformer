using System.Collections;
using DG.Tweening;
using Services.SceneLoading;
using Static;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Zenject.SpaceFighter;

namespace DefaultNamespace.Views
{
    public class PortalView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioClip _tpSound;

        private ILoadingPipelineService _service;
        
        [Inject]
        public void Construct(ILoadingPipelineService service)
        {
            _service = service;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            var player = collision.gameObject;
            var components = player.GetComponents<MonoBehaviour>();
            
            foreach (var component in components)
                Destroy(component);
            
            player.transform.DOScale(Vector3.zero, 0.5f)
                .OnComplete(() => AudioSource.PlayClipAtPoint(_tpSound, transform.position));
            
            player.transform.DOMove(_particleSystem.transform.position, 0.5f)
                .OnComplete(() => StartCoroutine(HandleSceneTransition()));
        }

        private IEnumerator HandleSceneTransition()
        {
            _particleSystem.Stop();
                
            while (_particleSystem.IsAlive(true))
                yield return null;
            
            yield return new WaitForSeconds(2f);

            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                _service.LoadGameScene(nextSceneIndex);
            else
                Application.Quit();
        }
    }
}