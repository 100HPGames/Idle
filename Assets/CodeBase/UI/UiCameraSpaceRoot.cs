using CodeBase.Tools;
using CodeBase.Tools.Helpers;
using UnityEngine;

namespace CodeBase.UI
{
    public class UiCameraSpaceRoot : MonoBehaviour, IService
    {
        [SerializeField] private Transform _flyItemParent;
        [SerializeField] private Canvas _canvas;
		
        public Transform FlyItemParent => _flyItemParent;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _canvas.worldCamera = Helper.Camera;
        }

        public float GetCanvasScaleFactor() => _canvas.scaleFactor;
    }
}