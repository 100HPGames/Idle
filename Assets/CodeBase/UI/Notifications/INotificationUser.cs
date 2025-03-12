using System;
using UI.MainMenu;

namespace CodeBase.UI.NotificationFolder
{
	public interface INotificationUser
	{
		public event Action<WindowType, bool> OnUpdateNotification;
	}
}