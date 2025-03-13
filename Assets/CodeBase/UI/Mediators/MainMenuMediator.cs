using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.MediatoerService;
using CodeBase.Tools.Helpers;
using UnityEngine;

namespace CodeBase.UI.Mediators
{
	public class MainMenuMediator : MonoBehaviour, IMediator
	{
		public event Action<IMediator> OnCleanUp;
		public event Action<WindowType> OnClickShrinkButton;

		[Header("Components")]
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private List<ShrinkButton> _shrinkButtons;

		private MediatorFactory _mediatorFactory;
		public GameObject GameObject => gameObject;
		public bool IsAdditiveMediator { get; set; } = true;
		public bool IsCameraSpace { get; }
		public void Show() => _canvasGroup.Show();
		public void Hide() => _canvasGroup.Hide();

		public void Construct()
		{
		}

		public void UpdateNotificationShrinkButton(WindowType windowType, bool show)
		{
			var shrinkButton = _shrinkButtons.FirstOrDefault(x => x.WindowType == windowType);
			shrinkButton!.ShowNotification(show);
		}

		private void Awake()
		{
			ClickShrinkButton(WindowType.SelectLevel, _shrinkButtons[2]);
			_shrinkButtons.ForEach(x => x.OnClick += ClickShrinkButton);
		}

		private void ClickShrinkButton(WindowType windowType, ShrinkButton button)
		{
			_shrinkButtons.ForEach(x => x.Shrink());
			button.Expand();
			OnClickShrinkButton?.Invoke(windowType);
		}

		private void OnDestroy()
		{
			_shrinkButtons.ForEach(x => x.OnClick -= ClickShrinkButton);
			OnCleanUp?.Invoke(this);
		}
	}
}