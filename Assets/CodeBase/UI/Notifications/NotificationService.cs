﻿using CodeBase.Infrastructure.Services.SaveService;
using CodeBase.Tools;

namespace CodeBase.UI.Notifications
{
	public class NotificationService : IService, ILoader, ISaver
	{
		private readonly PersistentData _persistentData;
		
		public NotificationService(PersistentData persistentData)
		{
			_persistentData = persistentData;
			LoadAllData(_persistentData);
		}

		public void Load(GameSave save)
		{
		
		}

		public void Save(GameSave save)
		{
			
		}
		
		private void LoadAllData(PersistentData persistentData) => persistentData.LoadToObject(this);

		private void SaveAllData(PersistentData persistentData) => persistentData.SaveFromObject(this);
	}
}