using System;
using UI.MainMenu;

namespace CodeBase.UI.NotificationFolder
{
	public interface INotificationUsed
	{
		public event Action<WindowType, bool> OnUpdateNotification;
	}
}