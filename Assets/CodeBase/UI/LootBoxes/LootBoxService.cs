using System;
using System.Collections.Generic;
using _Tools;
using CodeBase.Infrastructure;
using CodeBase.Tools;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.LootBoxShop;
using MoneyHandler;

namespace Loot
{
	public class LootBoxService : IService, IDisposable
	{
		public event Action<List<RewardDTO>, LootBoxType> OnLootBoxShow;

		private readonly List<LootBoxDTO> _lootBoxDataList;
		private readonly CurrencyService _currencyService;
		private readonly PurchaseService _purchaseService;

		public LootBoxService(StaticData staticData, CurrencyService currencyService, PurchaseService purchaseService)
		{
			_purchaseService = purchaseService;
			_currencyService = currencyService;
			_lootBoxDataList = staticData.GameDTO.ShopDto.LootBoxesDTOs;
			_purchaseService.OnLootBoxShow += PurchaseLootBox;
		}

		public void Dispose() => _purchaseService.OnLootBoxShow -= PurchaseLootBox;

		private void PurchaseLootBox((CurrencyType priceCurrencyType, int priceCurrencyValue) priceTuple,
		                             LootBoxType lootBoxType)
		{
			if (!_currencyService.TrySubtractResource(priceTuple.priceCurrencyType, priceTuple.priceCurrencyValue))
				return;

			var rewardLootBoxDrop = AcquireLootBoxDrop(lootBoxType);

			OnLootBoxShow?.Invoke(rewardLootBoxDrop, lootBoxType);
		}

		public List<RewardDTO> AcquireLootBoxDrop(LootBoxType type)
		{
			var rewards = new List<RewardDTO>();
			return rewards;
		}
	}
}