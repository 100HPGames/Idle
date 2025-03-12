using CodeBase.Tools.Helpers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.NotificationFolder
{
	public class NotificationUi : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Transform _transformToBounce;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Image _image;
		[SerializeField] private TMP_Text _notificationText;

		[Header("Settings")]
		[SerializeField] private float _endValue = 1.35f;
		[SerializeField] private float _durationBounce = 0.5f;

		private Tween _bounceTween;
		private bool _isActivateNotification;

		public bool IsActivateNotification => _isActivateNotification;

		public void Show()
		{
			_canvasGroup.Show();
			_bounceTween?.Kill();
			_transformToBounce.localScale = Vector3.one;
			_bounceTween = _transformToBounce.DOScale(_endValue, _durationBounce)
			                                 .SetLoops(-1, LoopType.Yoyo)
			                                 .SetLink(_transformToBounce.gameObject);
			_isActivateNotification = true;
		}

		public void Hide()
		{
			_bounceTween?.Kill();
			_transformToBounce.localScale = Vector3.one;
			_canvasGroup.Hide();
			_isActivateNotification = false;
		}

		public void SetIcon(Sprite sprite) => _image.sprite = sprite;

		public void SetText(string text)
		{
			if (_notificationText != default)
				_notificationText.SetText(text);
		}
	}
}