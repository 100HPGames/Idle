using System;
using System.Collections.Generic;
using _Tools;
using CodeBase.UI.Mediators;
using MoneyHandler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.LootBoxShop
{
	public class LootBoxShopPlankView : MonoBehaviour
	{
		public event Action<(CurrencyType, int), LootBoxType> OnBuyLootBox;

		[Header("Components")]
		[SerializeField] private Image _lootBoxIcon;
		[SerializeField] private TMP_Text _lootBoxName;
		[SerializeField] private TMP_Text _buyButtonText;
		[SerializeField] private SimpleButton _buyButton;

		[Header("Icons")]
		[SerializeField] private List<Sprite> _lootBoxIcons;

		private LootBoxDTO _lootBoxDto;
		private PurchaseService _purchaseService;

		public void Init(LootBoxDTO lootBoxDto, PurchaseService purchaseService)
		{
			_purchaseService = purchaseService;
			_lootBoxDto = lootBoxDto;

			_buyButton.OnClick += BuyLootBox;
		}

		public void UpdateBuyState()
		{
			var interactable = _purchaseService.GetResourceCount(_lootBoxDto.PriceType) >= _lootBoxDto.PriceValue;
			_buyButton.SetInteractable(interactable); ;
			_buyButtonText.alpha = interactable ? 1f : 0.5f;
			SetView();
		}

		private void SetView()
		{
			_lootBoxName.SetText(_lootBoxDto.LootBoxType.ToString());
			_buyButtonText.SetText(GetCostText());

			var index = 0;
			switch (_lootBoxDto.LootBoxType)
			{
				case LootBoxType.RARE:
					index = 0;
					break;
				case LootBoxType.EPIC:
					index = 1;
					break;
			}

			_lootBoxIcon.sprite = _lootBoxIcons[index];
		}

		private string GetCostText()
		{
			var currencyType = _lootBoxDto.PriceType;
			var price = _lootBoxDto.PriceValue;

			var spriteAsset = "";
			switch (currencyType)
			{
				case CurrencyType.Soft:
					spriteAsset = "Coin";
					break;
				case CurrencyType.Hard:
					spriteAsset = "Gem";
					break;
			}

			var totalCurrency = $"<sprite=\"{spriteAsset}\", index=0> {price}";
			return $"{totalCurrency}";
		}

		private void OnDestroy() => _buyButton.OnClick -= BuyLootBox;

		private void BuyLootBox()
			=> OnBuyLootBox?.Invoke((_lootBoxDto.PriceType, _lootBoxDto.PriceValue), _lootBoxDto.LootBoxType);
	}
}