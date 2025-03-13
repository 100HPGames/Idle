using System;
using CodeBase.Components.Interactions;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.MediatoerService;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.SimpleMessenger;
using UnityEngine;

namespace CodeBase.UI.Mediators
{
	public class BattleMediator : MonoBehaviour, IMediator, IDisposable
	{
		public event Action<IMediator> OnCleanUp;
	
		[Header("Components")]
		[SerializeField] private CanvasGroup _canvasGroup;
		
		[Header("Buttons")]
		[SerializeField] private SimpleButton _showBookButton;
		[SerializeField] private SimpleButton _showMenuButton;

		private Messenger _messenger;

		public GameObject GameObject => gameObject;

		public void Construct(Messenger messenger)
		{
			_messenger = messenger;
		}

		public void Show() => _canvasGroup.Show();
		public void Hide() => _canvasGroup.Hide();

		public bool IsAdditiveMediator { get; set; } = false;
		public bool IsCameraSpace { get; }

		public void Dispose()
		{
			OnCleanUp?.Invoke(this);
			Destroy(gameObject);
		}
		
		private void ShowMenu() => _messenger.Pub(new EndLevelMessage());
	}
}