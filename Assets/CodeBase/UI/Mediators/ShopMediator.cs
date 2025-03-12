using System;
using CodeBase.Infrastructure;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.LootBoxShop;
using CodeBase.UI.NotificationFolder;
using CodeBase.UI.Shop;
using MoneyHandler;
using UI.MainMenu;
using UnityEngine;
using static CodeBase.Tools.Helpers.CanvasGroupExtension;

namespace CodeBase.UI.Mediators
{
	public class ShopMediator : MonoBehaviour, IMediator, INotificationUsed
	{
		public event Action<IMediator> OnCleanUp;
		public event Action<WindowType, bool> OnUpdateNotification;

		[Header("Components")]
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private LootBoxShopView _lootBoxShopView;
		[SerializeField] private CurrencyElementView _softCurrencyElementView;
		[SerializeField] private CurrencyElementView _recipeCurrencyElementView;

		[Header("Animation")]
		[SerializeField] private float _durationShow = 0.25f;
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private WindowAnimationType _animationType;

		private Vector3 _cachedPosition;
		private AssetProvider _assetProvider;
		private PurchaseService _purchaseService;

		public GameObject GameObject => gameObject;

		public bool IsAdditiveMediator { get; set; } = false;
		public bool IsCameraSpace { get; }

		public void Show()
		{
			_canvasGroup.Show(_durationShow,
			                  rectTuple: (_cachedPosition, _rectTransform),
			                  animationType: _animationType);
			UpdateShopElements();
		}

		public void Hide() => _canvasGroup.Hide();

		public void Construct(PurchaseService purchaseService, AssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
			_purchaseService = purchaseService;

			BuildLootBoxShop();
			BuildCurrencyShops();
		}

		private void Awake() => _cachedPosition = _rectTransform.position;

		private void BuildLootBoxShop()
		{
			_lootBoxShopView.Construct(_assetProvider, _purchaseService);
			_lootBoxShopView.OnBuyLootBox += AddNewLootBox;
		}

		private void BuildCurrencyShops()
		{
			_softCurrencyElementView.Construct(_assetProvider, _purchaseService);
			_recipeCurrencyElementView.Construct(_assetProvider, _purchaseService);
			_softCurrencyElementView.OnBuyCurrency += TryPurchaseSoftCurrency;
		}

		private void OnDestroy()
		{
			_lootBoxShopView.OnBuyLootBox -= AddNewLootBox;
			_softCurrencyElementView.OnBuyCurrency -= TryPurchaseSoftCurrency;
		}

		private void UpdateShopElements()
		{
			_lootBoxShopView.UpdatePlanks();
			_softCurrencyElementView.UpdatePlanks();
		}

		private void AddNewLootBox((CurrencyType, int) priceTuple, LootBoxType lootBoxType)
		{
			_purchaseService.PurchaseLootBox(priceTuple, lootBoxType);
			_lootBoxShopView.UpdatePlanks();
		}

		private void TryPurchaseSoftCurrency((CurrencyType addedCurrencyType, float addedCurrencyValue) addedTuple,
		                                     (CurrencyType priceCurrencyType, float priceCurrencyValue) priceTuple)
		{
			_purchaseService.PurchaseCurrency(addedTuple, priceTuple);
			UpdateShopElements();
		}
	}
}