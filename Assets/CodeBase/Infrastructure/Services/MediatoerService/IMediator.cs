using System;
using UI.MainMenu;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public interface IMediator
	{
		public event Action<IMediator> OnCleanUp;
		public GameObject GameObject { get; }
		public void Show();
		public void Hide();
		public bool IsAdditiveMediator { get; }
		public bool IsCameraSpace { get; }
	}
}