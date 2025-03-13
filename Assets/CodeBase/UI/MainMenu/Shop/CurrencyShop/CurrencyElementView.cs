using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using CodeBase.Tools.CurrencyHandler;
using CodeBase.Tools.Helpers;
using CodeBase.UI.Mediators;
using UnityEngine;

namespace CodeBase.UI.MainMenu.Shop.CurrencyShop
{
	public class CurrencyElementView : MonoBehaviour
	{
		public event
			Action<(CurrencyType addedCurrencyType, float addedCurrencyValue), (CurrencyType priceCurrencyType, float
				priceCurrencyValue)> OnBuyCurrency;

		[Header("Components")]
		[SerializeField] private Transform _currencyPlankParent;
		[SerializeField] private List<CurrencyType> _includeTypes;

		private readonly List<CurrencyElementPlankView> _currencyPlankViews = new();

		public void Construct(AssetProvider assetProvider, PurchaseService purchaseService)
		{
			foreach (var currencyShopDto in purchaseService.ShopCurrencyElementsList
			                                               .Where(x => _includeTypes.Contains(x.CurrencyType))
			                                               .ToList())
			{
				var currencyElementPlankView
					= Instantiate(assetProvider.GetCurrencyElementPlankView(), _currencyPlankParent);
				currencyElementPlankView.Init(currencyShopDto,
				                              purchaseService);
				_currencyPlankViews.TryAdd(currencyElementPlankView);
			}

			UpdatePlanks();

			_currencyPlankViews.ForEach(x => x.OnBuyCurrency += BuyCurrency);
		}

		private void OnDestroy() => _currencyPlankViews.ForEach(x => x.OnBuyCurrency -= BuyCurrency);

		private void BuyCurrency((CurrencyType addedCurrencyType, float addedCurrencyValue) addedTuple,
		                         (CurrencyType priceCurrencyType, float priceCurrencyValue) priceTuple)
			=> OnBuyCurrency?.Invoke(addedTuple, priceTuple);

		public void UpdatePlanks()
		{
			foreach (var spellShopPlankView in _currencyPlankViews)
				spellShopPlankView.UpdateBuyState();
		}
	}
}