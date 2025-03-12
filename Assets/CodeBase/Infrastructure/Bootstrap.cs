using CodeBase.InputService;
using CodeBase.Tools;
using CodeBase.Tools.SimpleMessenger;
using CodeBase.UI;
using CodeBase.UI.FlyService;
using CodeBase.UI.Mediators;
using CodeBase.UI.NotificationFolder;
using Cysharp.Threading.Tasks;
using Loot;
using MoneyHandler;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class Bootstrap : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Game _game;
		[SerializeField] private PrefabHub _prefabHub;
		[SerializeField] private LoadingCurtain _loadingCurtain;
		[SerializeField] private TouchInputService _touchInputService;
		[SerializeField] private SwipeInputService _swipeInputService;
		[SerializeField] private bool _wildMagick;

		private ServiceLocator _serviceLocator;

		private async void Start()
		{
			Application.targetFrameRate = 144;
			DontDestroyOnLoad(gameObject);
			_loadingCurtain.Show();
			_serviceLocator = new ServiceLocator();
			await RegisterStaticData();
			RegisterCurtain();
			RegisterMessenger();
			RegisterPersistentData();
			RegisterAssetProvider();
			RegisterCurrencyService();
			await RegisterMediatorFactory();
			RegisterPurchaseService();
			RegisterNotificationService();
			RegisterTouchInputService();
			RegisterSwipeService();
			RegisterFlyItemService();
			RegisterLootBoxService();

			await UniTask.Yield();

			_game.Initialize(_serviceLocator);
		}

		private void RegisterCurtain()
		{
			_serviceLocator.Reg(_loadingCurtain);
		}

		private async UniTask RegisterStaticData()
		{
			var staticData = new StaticData();
			await staticData.Initialize();
			_serviceLocator.Reg(staticData);
		}

		private void RegisterPersistentData()
		{
			var persistentData = new PersistentData();
			persistentData.Initialize();
			_serviceLocator.Reg(persistentData);
		}

		private void RegisterAssetProvider()
		{
			var assetProvider = new AssetProvider();
			assetProvider.Initialize(_prefabHub);
			_serviceLocator.Reg(assetProvider);
		}

		private void RegisterTouchInputService()
		{
			_serviceLocator.Reg(_touchInputService);
		}

		private void RegisterSwipeService()
		{
			_serviceLocator.Reg(_swipeInputService);
			var swipeInputService = _serviceLocator.Get<SwipeInputService>();
			swipeInputService.Initialize(_touchInputService);
		}

		private void RegisterFlyItemService()
		{
			var flyItemService = new FlyItemService();
			flyItemService.Initialize(_serviceLocator.Get<AssetProvider>(), _serviceLocator.Get<UiOverlayRoot>());
			_serviceLocator.Reg(flyItemService);
		}

		private void RegisterCurrencyService()
		{
			var currencyService = new CurrencyService(_serviceLocator.Get<PersistentData>());
			_serviceLocator.Reg(currencyService);
		}

		private void RegisterNotificationService()
		{
			var notificationService = new NotificationService(_serviceLocator.Get<PersistentData>());
			_serviceLocator.Reg(notificationService);
		}

		private void RegisterMessenger() => _serviceLocator.Reg(new Messenger());

		
		private async UniTask RegisterMediatorFactory()
		{
			var mediatorFactory = new MediatorFactory(_serviceLocator.Get<AssetProvider>(), _serviceLocator);
			_serviceLocator.Reg(mediatorFactory);
			await mediatorFactory.CreateUIRoot();
		}

		private void RegisterPurchaseService()
		{
			_serviceLocator.Reg(new PurchaseService(_serviceLocator.Get<PersistentData>(),
			                                        _serviceLocator.Get<CurrencyService>(),
			                                        _serviceLocator.Get<StaticData>()));
		}
		
		private void RegisterLootBoxService()
		{
			var lootBx = new LootBoxService(_serviceLocator.Get<StaticData>(),
			                                _serviceLocator.Get<CurrencyService>(),
			                                _serviceLocator.Get<PurchaseService>());
			_serviceLocator.Reg(lootBx);
		}
	}
}