using System;

namespace CodeBase.UI.Notifications
{
	public interface INotificationUser
	{
		public event Action<WindowType, bool> OnUpdateNotification;
	}
}