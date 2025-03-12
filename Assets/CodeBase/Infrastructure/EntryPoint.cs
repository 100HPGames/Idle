using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        private const string PreloaderName = "Preloader";
        [SerializeField] private Bootstrap _bootstrapPrefab;
        private static bool _isStarted;

        private async void Awake()
        {
            if (_isStarted)
                return;
            
            _isStarted = true;
            
            if (SceneManager.GetActiveScene().name != PreloaderName)
            {
                await SceneManager.LoadSceneAsync(PreloaderName, LoadSceneMode.Single).ToUniTask();
                return;
            }
            
            Bootstrap bootstrapper = FindObjectOfType<Bootstrap>();
            if (bootstrapper != null)
                return;

            Instantiate(_bootstrapPrefab); 
        }
    }
}