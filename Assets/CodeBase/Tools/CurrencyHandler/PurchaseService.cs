using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveService;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.LootBoxes;
using CodeBase.UI.Mediators;

namespace CodeBase.Tools.CurrencyHandler
{
	public class PurchaseService : IService, ILoader, ISaver, IDisposable
	{
		public event Action<(CurrencyType, int), LootBoxType> OnLootBoxShow;

		private readonly StaticData _staticData;
		private readonly LootBoxService _lootBoxService;
		private readonly CurrencyService _currencyService;
		private readonly List<LootBoxDTO> _shopLootBoxList = new();
		private readonly List<CurrencyShopDTO> _shopCurrencyElementsList = new();
		public List<LootBoxDTO> ShopLootBoxList => _shopLootBoxList;
		public List<CurrencyShopDTO> ShopCurrencyElementsList => _shopCurrencyElementsList;

		public PurchaseService(PersistentData persistentData, CurrencyService currencyService, StaticData staticData)
		{
			_currencyService = currencyService;
			_staticData = staticData;

			foreach (var currencyShopDto in staticData.GameDTO.ShopDto.CurrencyShopDTO)
				_shopCurrencyElementsList.TryAdd(currencyShopDto);

			foreach (var lootBoxDto in staticData.GameDTO.ShopDto.LootBoxesDTOs)
				_shopLootBoxList.TryAdd(lootBoxDto);

			LoadAllData(persistentData);
		}

		public void Dispose()
		{
			
		}
		
		public void Load(GameSave save)
		{

		}
		
		public void Save(GameSave save)
		{

		}

		public float GetResourceCount(CurrencyType currencyType) => _currencyService.GetResourceCount(currencyType);

		public void PurchaseCurrency((CurrencyType addedCurrencyType, float addedCurrencyValue) addedTuple,
		                             (CurrencyType priceCurrencyType, float priceCurrencyValue) priceTuple)
		{
			if (_currencyService.TrySubtractResource(priceTuple.priceCurrencyType, (int) priceTuple.priceCurrencyValue))
				_currencyService.AddResource(addedTuple.addedCurrencyType, (int) addedTuple.addedCurrencyValue);
		}

		public void PurchaseLootBox((CurrencyType priceCurrencyType, int priceCurrencyValue) priceTuple, LootBoxType lootBoxType)
			=> OnLootBoxShow?.Invoke(priceTuple, lootBoxType);
		
		private void LoadAllData(PersistentData persistentData) => persistentData.LoadToObject(this);

		private void SaveAllData(PersistentData persistentData) => persistentData.SaveFromObject(this);
	}
}