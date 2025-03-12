using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.SaveService;
using CodeBase.Tools;
using CodeBase.Tools.Helpers;
using UnityEngine;

namespace MoneyHandler
{
	public class CurrencyService : IService, ISaver, ILoader
	{
		public event Action<CurrencyType, int> OnValueSet;
		public event Action<CurrencyType, int, Vector3, bool, bool> OnValueWithPayloadAdded;
		public event Action<CurrencyType, int> OnValueAdded;
		public event Action<CurrencyType, int> OnValueSubtracted;
		public event Action<CurrencyType, int> OnValueChanged;

		private readonly PersistentData _persistentData;
		private Dictionary<CurrencyType, int> _currencyMap = new();
		private readonly Vector3 _middleScreenPosition = new(Screen.width / 2f, Screen.height / 2f);

		public CurrencyService(PersistentData persistentData)
		{
			_persistentData = persistentData;
			LoadAllData(_persistentData);
		}

		public bool AddResource(CurrencyType type,
		                        int addedValue,
		                        bool autoSave = true,
		                        Vector3 screenPosition = default,
		                        bool fly = true,
		                        bool flyLeft = false)
		{
			if (addedValue < 0)
				return false;

			_currencyMap.TryAdd(type, addedValue);

			if (screenPosition == default)
				screenPosition = _middleScreenPosition;
			OnValueWithPayloadAdded?.Invoke(type, addedValue, screenPosition, fly, flyLeft);
			OnValueAdded?.Invoke(type, addedValue);
			var value = _currencyMap[type] += addedValue;
			OnValueChanged?.Invoke(type, value);
			if (autoSave)
				SaveAllData(_persistentData);
			return true;
		}

		public bool TrySubtractResource(CurrencyType type, int subtractValue, bool autoSave = true)
		{
			if (subtractValue < 0)
				return false;

			if (!_currencyMap.TryGetValue(type, out var value))
				return false;

			if (value < subtractValue)
				return false;

			OnValueSubtracted?.Invoke(type, subtractValue);
			_currencyMap[type] = value -= subtractValue;
			OnValueChanged?.Invoke(type, value);
			if (autoSave)
				SaveAllData(_persistentData);
			return true;
		}

		public int GetResourceCount(CurrencyType type)
		{
			_currencyMap.TryAdd(type, default);

			return _currencyMap[type];
		}

		public void Save(GameSave gameSave) => gameSave.CurrencyMap = _currencyMap;

		public void Load(GameSave save)
		{
			_currencyMap = save.CurrencyMap;
			foreach (var currencyMapKey in _currencyMap.Keys)
				OnValueSet?.Invoke(currencyMapKey, _currencyMap[currencyMapKey]);
		}

		private void LoadAllData(PersistentData persistentData) => persistentData.LoadToObject(this);
		private void SaveAllData(PersistentData persistentData) => persistentData.SaveFromObject(this);
	}
}