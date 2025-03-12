using CodeBase.Tools;
using CodeBase.Tools.Helpers;
using UnityEngine;

namespace CodeBase.UI
{
	public class UiOverlayRoot : MonoBehaviour, IService
	{
		[SerializeField] private Transform _flyItemParent;
		[SerializeField] private Canvas _canvas;
		
		public Transform FlyItemParent => _flyItemParent;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public float GetCanvasScaleFactor() => _canvas.scaleFactor;
	}
}