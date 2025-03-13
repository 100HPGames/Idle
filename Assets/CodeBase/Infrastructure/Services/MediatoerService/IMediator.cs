using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.MediatoerService
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