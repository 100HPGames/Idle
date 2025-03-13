using CodeBase.Tools.Helpers;
using CodeBase.UI.Mediators;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Tools.CurrencyHandler
{
	public class CurrencyView : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Image _resourceIcon;
		[SerializeField] private CanvasGroup _elementCanvas;
		[SerializeField] private TextMeshProUGUI _resourceCountText;

		private bool _shrink;
		private bool _autoHide;
		private bool _isShowForUpdate;
		private int _currentResourceCount;

		private CurrencyData _resourceData;
		private CurrencyService _currencyService;
		private bool _isFly;

		public Image ResourceIcon => _resourceIcon;
		public CurrencyType Type => _resourceData.Type;
		public Transform Transform => _resourceIcon.transform;

		public void Show()
		{
			_elementCanvas.Show();
			_isShowForUpdate = false;
		}

		public void Hide()
		{
			_elementCanvas.Hide();
			_isShowForUpdate = true;
		}

		public void Init(CurrencyData resourceData, bool autoHide, bool shrink, CurrencyService currencyService)
		{
			_currencyService = currencyService;
			_resourceData = resourceData;
			_autoHide = autoHide;
			_shrink = shrink;
			SetIcon();
			UpdateResourceMetaCount();
		}

		public void UpdateResourceUiView(int value)
		{
			_currentResourceCount += value;
			_elementCanvas.Show();

			if (!_isFly)
				UpdateView();
		}

		public void SetFly(bool isFly) => _isFly = isFly;

		public void UpdateView()
		{
			ScaleIcon();
			RefreshText();
		}

		public void SetCanvasShow(bool force = false)
		{
			gameObject.SetActive(!(_autoHide && !(force || _currentResourceCount > 0)));
		}

		public void SubtractResource(int value)
		{
			_currentResourceCount -= value;
			ScaleIcon();
			RefreshText();
		}

		public void SetResourceCount(int value)
		{
			_currentResourceCount = value;
			RefreshText();
		}

		private void RefreshText()
		{
			var showCount = Mathf.Clamp(_currentResourceCount, 0, int.MaxValue);
			var text = _shrink ? showCount.ToShortString() : showCount.ToString();
			_resourceCountText.text = text;
			SetCanvasShow();
		}

		private void SetIcon()
		{
			if (_resourceData == null)
				return;

			_resourceIcon.sprite = _resourceData.ResourceIcon;
		}

		private void ScaleIcon()
		{
			_resourceIcon.transform.DOKill();
			_resourceIcon.transform
			             .DOScale(1.35f, 0.25f)
			             .SetEase(Ease.OutQuint)
			             .OnComplete(() => _resourceIcon.transform
			                                            .DOScale(1f, 0.3f)
			                                            .OnComplete(() =>
			                                            {
				                                            if (_isShowForUpdate && !_isFly)
					                                            _elementCanvas.Hide(delay: 0.25f);
			                                            }));
		}

		private void UpdateResourceMetaCount()
		{
			_currentResourceCount = _currencyService.GetResourceCount(_resourceData.Type);
			RefreshText();
		}
	}
}