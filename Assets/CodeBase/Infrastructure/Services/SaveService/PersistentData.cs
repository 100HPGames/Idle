using CodeBase.Infrastructure.SaveService;
using CodeBase.Tools;
using CodeBase.Tools.StaticDataLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class PersistentData : IService, ISaveHandler, ILoadHandler
	{
		private GameSave _gameSave;

		public void Initialize()
		{
			var text = PlayerPrefs.GetString(GameConst.PersistentKey, "");
			if (string.IsNullOrEmpty(text) == false)
			{
				_gameSave = JsonConvert.DeserializeObject<GameSave>(text);
				return;
			}

			_gameSave = new GameSave();
		}

		private void Save()
		{
			var text = JsonConvert.SerializeObject(_gameSave, Formatting.Indented);
			PlayerPrefs.SetString(GameConst.PersistentKey, text);
		}

		public void LoadToObject(ILoader loader)
		{
			loader.Load(_gameSave);
		}

		public void SaveFromObject(ISaver saver)
		{
			saver.Save(_gameSave);
			Save();
		}
	}
}