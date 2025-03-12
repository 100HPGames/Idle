using CodeBase.Tools;
using CodeBase.Tools.Helpers;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, IService
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.Show();
        }

        public void Hide() => _canvasGroup.Hide(0.35f, 0f, () => gameObject.SetActive(false));
    }
}