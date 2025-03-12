using System;
using System.Collections.Generic;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.Mediators;
using MoneyHandler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Shop
{
	public class CurrencyElementPlankView : MonoBehaviour
	{
		public event
			Action<(CurrencyType addedCurrencyType, float addedCurrencyValue), (CurrencyType priceCurrencyType, float
				priceCurrencyValue)> OnBuyCurrency;

		[Header("Components")]
		[SerializeField] private Image _icon;
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _countText;
		[SerializeField] private TMP_Text _buyButtonText;
		[SerializeField] private SimpleButton _buyButton;

		[Header("Sprites")]
		[SerializeField] private List<Sprite> _currencySprites;

		private PurchaseService _purchaseService;
		private CurrencyShopDTO _currencyShopDto;

		public void UpdateBuyState()
		{
			var checkActivate = _purchaseService.GetResourceCount(_currencyShopDto.PriceType) >= _currencyShopDto.Price;

			_buyButton.SetInteractable(checkActivate);
			_buyButtonText.alpha = checkActivate ? 1 : 0.5f;
		}

		public void Init(CurrencyShopDTO currencyShopDto, PurchaseService purchaseService)
		{
			_purchaseService = purchaseService;
			_currencyShopDto = currencyShopDto;

			SetView();
			_buyButton.OnClick += BuyCurrency;
		}

		private void OnDestroy() => _buyButton.OnClick -= BuyCurrency;

		private void BuyCurrency()
			=> OnBuyCurrency?.Invoke((_currencyShopDto.CurrencyType, _currencyShopDto.Count),
			                         (_currencyShopDto.PriceType, _currencyShopDto.Price));

		private void SetView()
		{
			_nameText.SetText($"{_currencyShopDto.CurrencyType}");
			_countText.SetText($"{_currencyShopDto.Count}");

			var spriteAsset = "";
			var index = 0;

			spriteAsset = GetSpriteAsset(spriteAsset);
			index = GetIconIndex(index);

			_buyButtonText.SetText($"<sprite=\"{spriteAsset}\", index=0> {_currencyShopDto.Price}");
			_icon.sprite = _currencySprites[index];
		}

		private int GetIconIndex(int index)
		{
			switch (_currencyShopDto.CurrencyType)
			{
				case CurrencyType.Soft:
					index = 1;
					break;
				case CurrencyType.Hard:
					index = 2;
					break;
				default:
					index = 0;
					break;
			}

			return index;
		}

		private string GetSpriteAsset(string spriteAsset)
		{
			switch (_currencyShopDto.PriceType)
			{
				case CurrencyType.Soft:
					spriteAsset = "Coin";
					break;
				case CurrencyType.Hard:
					spriteAsset = "Gem";
					break;
			}

			return spriteAsset;
		}
	}
}