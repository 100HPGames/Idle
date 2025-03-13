using System;
using CodeBase.UI.Notifications;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.UI
{
	public class ShrinkButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public event Action<WindowType, ShrinkButton> OnClick;

		[Header("Components")]
		[SerializeField] private Button _button;
		[SerializeField] private Image _buttonIcon;
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private NotificationUi _notificationUi;
		[SerializeField] private GameObject _disabledView;
		[SerializeField] private GameObject _clickedView;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private WindowType _windowType;

		[Header("Settings")]
		[SerializeField] private float _timer;
		[Range(1, 2), SerializeField] private float _expandMultiplier;
		[SerializeField] private bool _hideTextWhenShrink;

		private bool _isInteractable;
		private bool _isSelected;
		private Vector2 _originalSize;
		private Tween _changeSizeTween;
		private bool _haveDefaultSize = false;
		private TweenerCore<Vector2, Vector2, VectorOptions> _sizeTweener;

		public WindowType WindowType => _windowType;

		public void SetState(bool isInteractable)
		{
			_isInteractable = isInteractable;
			_disabledView.SetActive(_isInteractable == false);
		}

		public void Show()
		{
			_buttonIcon.gameObject.SetActive(true);
			_button.image.raycastTarget = true;
		}

		public void Hide()
		{
			_buttonIcon.gameObject.SetActive(false);
			_button.image.raycastTarget = false;
		}

		[Button]
		public void Shrink(bool immedeate = false)
		{
			SetSDefaultSize();
			_isSelected = false;
			_clickedView.SetActive(false);
			KillSizeTweener();
			_sizeTweener = DOTween.To(() => _rectTransform.rect.size, ChangeButtonSize, _originalSize, _timer);
			if (_hideTextWhenShrink)
				_name.DOFade(0, immedeate ? 0 : 0.1f);
		}

		[Button]
		public void Expand(bool immedeate = false)
		{
			SetSDefaultSize();
			_isSelected = true;
			_clickedView.SetActive(true);
			KillSizeTweener();
			_sizeTweener = DOTween.To(() => _rectTransform.rect.size,
			                          ChangeButtonSize,
			                          _originalSize * _expandMultiplier,
			                          immedeate ? 0 : _timer);
			
			if (_hideTextWhenShrink)
				_name.DOFade(1, immedeate ? 0 : 0.35f);
		}

		private void PerformClick(bool selfClick)
		{
			if (_isSelected)
				return;

			OnClick?.Invoke(_windowType, this);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (_isSelected)
				return;

			_clickedView.SetActive(true);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (_isSelected)
				return;

			_clickedView.SetActive(false);
		}

		public void ShowNotification(bool show)
		{
			if (show)
				_notificationUi.Show();
			else
				_notificationUi.Hide();
		}

		private void Awake() => _button.onClick.AddListener(() => PerformClick(false));

		private void OnDestroy() => _button.onClick.RemoveListener(() => PerformClick(false));

		private void SetSDefaultSize()
		{
			if (_haveDefaultSize == false)
			{
				_haveDefaultSize = true;
				_originalSize = _rectTransform.rect.size;
			}
		}
		
		private void KillSizeTweener()
		{
			if (_sizeTweener != null)
			{
				_sizeTweener.Kill();
				_sizeTweener = null;
			}
		}

		private void ChangeButtonSize(Vector2 targetSize)
		{
			Vector2 oldSize = _rectTransform.rect.size;
			Vector2 deltaSize = targetSize - oldSize;
			Vector2 pivot = _rectTransform.pivot;
			_rectTransform.offsetMin -= new Vector2(deltaSize.x * pivot.x, deltaSize.y * pivot.y);
			_rectTransform.offsetMax
				+= new Vector2(deltaSize.x * (1f - _rectTransform.pivot.x), deltaSize.y * (1f - pivot.y));
		}
	}
}