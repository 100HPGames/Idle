using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.MonoBehaivours;
using CodeBase.Tools;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.FlyService;
using CodeBase.UI.LootBoxShop;
using CodeBase.UI.Shop;
using Cysharp.Threading.Tasks;
using UI.ChestUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure
{
	public class AssetProvider : IService
	{
		private PrefabHub _prefabHub;
		private readonly Dictionary<string, Object> _completedCashe = new();
		public void Initialize(PrefabHub prefabHub) => _prefabHub = prefabHub;

		public async Task LoadSceneSingle(string sceneName) => await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

		public async UniTask<T> Load<T>(string address) where T : Object
		{
			if (_completedCashe.TryGetValue(address, out Object loadedObject))
				return loadedObject as T;

			Object result = await Resources.LoadAsync<T>(address).ToUniTask();
			_completedCashe.TryAdd(address, result);
			return result as T;
		}

		public async Task LoadSceneAdditive(string sceneName) => await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		public async Task UnloadSceneAsync(string sceneName) => await SceneManager.UnloadSceneAsync(sceneName);
		public FlyItemView GetFlyItemView() => _prefabHub.FlyItemView;
		public PlayerView GetPlayerView() => _prefabHub.PlayerView;
		public EnemyAbstract GetEnemyView(EnemyId enemyType) => _prefabHub.GetEnemyView(enemyType);
		public DamageView GetDamageViewPrefab() => _prefabHub.DamageViewPrefab;
		public CurrencyElementPlankView GetCurrencyElementPlankView() => _prefabHub.CurrencyElementPlankView;
		public LootBoxOpenView GetLootBoxPrefab(LootBoxType lootBoxType) => _prefabHub.LootBoxes.FirstOrDefault(v => v.LootBoxType == lootBoxType);
	}
}