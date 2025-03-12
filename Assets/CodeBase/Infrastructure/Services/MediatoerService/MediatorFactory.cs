using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.InputService;
using CodeBase.Tools;
using CodeBase.Tools.SimpleMessenger;
using CodeBase.UI.FlyService;
using CodeBase.UI.NotificationFolder;
using Cysharp.Threading.Tasks;
using MoneyHandler;
using UI.ChestUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Mediators
{
	public class MediatorFactory : IService
	{
		private const string UIOverlayRootPath = "UiOverlayRoot";
		private const string UICameraRootPath = "UiCameraRoot";
		private const string AdditionalPath = "GameData/GUI/";
		private readonly AssetProvider _assetProvider;
		private readonly ServiceLocator _serviceLocator;
		private readonly Dictionary<Type, IMediator> _mediators;
		private UiOverlayRoot _uiOverlayRoot;
		private UiCameraSpaceRoot _uiCameraRoot;
		private bool _spawnRoots;

		public MediatorFactory(AssetProvider assetProvider, ServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
			_assetProvider = assetProvider;
			_mediators = new Dictionary<Type, IMediator>();
		}

		public async UniTask CreateUIRoot()
		{
			GameObject prefab1 = await _assetProvider.Load<GameObject>(AdditionalPath + UIOverlayRootPath);
			GameObject prefab2 = await _assetProvider.Load<GameObject>(AdditionalPath + UICameraRootPath);
			_uiOverlayRoot = Object.Instantiate(prefab1).GetComponent<UiOverlayRoot>();
			_uiCameraRoot = Object.Instantiate(prefab2).GetComponent<UiCameraSpaceRoot>();
			_serviceLocator.Reg(_uiOverlayRoot);
			_serviceLocator.Reg(_uiCameraRoot);
			_spawnRoots = true;
		}

		public async UniTask<TMediator> Get<TMediator>() where TMediator : MonoBehaviour, IMediator
		{
			if (_mediators.TryGetValue(typeof(TMediator), out var mediator))
				return mediator as TMediator;

			if (_spawnRoots == false)
				await CreateUIRoot();

			TMediator mediatorGo = await _assetProvider.Load<TMediator>(AdditionalPath + typeof(TMediator).Name);
			Transform parent = mediatorGo.IsCameraSpace ? _uiCameraRoot.transform : _uiOverlayRoot.transform;
			TMediator instance = Object.Instantiate(mediatorGo, parent);
			InitMediator(instance);
			_mediators.Add(instance.GetType(), instance);
			instance.OnCleanUp += CleanUp;
			return instance;
		}

		private void InitMediator<TMediator>(TMediator instance) where TMediator : MonoBehaviour, IMediator
		{
			switch (instance)
			{
				case BattleMediator battleMediator:
					battleMediator.Construct(_serviceLocator.Get<Messenger>());
					break;
				case CurrencyMediator currencyMediator:
					currencyMediator.Construct(_serviceLocator.Get<CurrencyService>(),
					                           _serviceLocator.Get<FlyItemService>());
					break;
				case MainMenuMediator menuTabsMediator:
					menuTabsMediator.Construct();
					break;
				case ShopMediator shopMediator:
					shopMediator.Construct(_serviceLocator.Get<PurchaseService>(),
					                       _serviceLocator.Get<AssetProvider>());
					break;
				case FinishLevelMediator finishMediator:
					finishMediator.Construct(_serviceLocator.Get<Messenger>(), this,
					                         _serviceLocator.Get<CurrencyService>());
					break;
				case LootBoxMediator lootBoxMediator:
					lootBoxMediator.Construct(_serviceLocator.Get<AssetProvider>());
					break;
			}
		}

		public async UniTask Show<TMediator>(bool suppressAllWindows = false) where TMediator : MonoBehaviour, IMediator
		{
			var result = await Get<TMediator>();
			foreach (var mediatorPair in _mediators)
			{
				if (suppressAllWindows == false && mediatorPair.Value.IsAdditiveMediator)
					continue;

				mediatorPair.Value.Hide();
			}

			result.Show();
		}

		private void CleanUp(IMediator mediator)
		{
			mediator.OnCleanUp -= CleanUp;
			if (_mediators.ContainsKey(mediator.GetType()))
				_mediators.Remove(mediator.GetType());
		}
	}
}