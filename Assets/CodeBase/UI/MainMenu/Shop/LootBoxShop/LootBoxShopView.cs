using System;
using System.Collections.Generic;
using _Tools;
using CodeBase.Infrastructure;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.StaticDataLoader;
using MoneyHandler;
using UnityEngine;

namespace CodeBase.UI.LootBoxShop
{
	public class LootBoxShopView : MonoBehaviour
	{
		public event Action<(CurrencyType, int), LootBoxType> OnBuyLootBox;

		[Header("Components")]
		[SerializeField] private Transform _lootBoxPlankParent;

		private readonly List<LootBoxShopPlankView> _lootBoxPlankViews = new();

		public void Construct(AssetProvider assetProvider, PurchaseService purchaseService)
		{
			foreach (var lootBoxDto in purchaseService.ShopLootBoxList)
			{
				//var lootBoxElementPlankView = Instantiate();
				//lootBoxElementPlankView.Init(lootBoxDto, purchaseService);
				//_lootBoxPlankViews.TryAdd(lootBoxElementPlankView);
			}

			UpdatePlanks();

			_lootBoxPlankViews.ForEach(x => x.OnBuyLootBox += BuyLootBox);
		}

		public void UpdatePlanks()
		{
			foreach (var spellShopPlankView in _lootBoxPlankViews)
				spellShopPlankView.UpdateBuyState();
		}

		private void OnDestroy() => _lootBoxPlankViews.ForEach(x => x.OnBuyLootBox -= BuyLootBox);

		private static Rarity GetRarity(LootBoxDTO lootBoxDto)
		{
			var rarity = lootBoxDto.LootBoxType switch
			{
				LootBoxType.RARE => Rarity.Rare,
				LootBoxType.EPIC => Rarity.Epic,
				_                => default
			};
			return rarity;
		}

		private void BuyLootBox((CurrencyType, int) priceTuple, LootBoxType lootBoxType)
			=> OnBuyLootBox?.Invoke(priceTuple, lootBoxType);
	}
}