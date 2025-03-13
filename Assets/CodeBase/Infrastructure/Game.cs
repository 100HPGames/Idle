using System.Threading.Tasks;
using CodeBase.Infrastructure.GameStates;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.MediatoerService;
using CodeBase.Tools;
using CodeBase.Tools.StateMachine;
using CodeBase.UI.FlyService;
using CodeBase.UI.LootBoxes;
using CodeBase.UI.Mediators;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class Game : MonoBehaviour
	{
		private bool _initialized;
		private ServiceLocator _services;
		private StateMachine _stateMachine;
		private MediatorFactory _mediatorFactory;
		private ISelfCompleteState _loadMenuState;
		private CurrencyMediator _currencyMediator;

		public async void Initialize(ServiceLocator serviceLocator)
		{
			_services = serviceLocator;
			_mediatorFactory = _services.Get<MediatorFactory>();
			_stateMachine = new StateMachine();
			
			await InitAdditionalMediators();

			_loadMenuState = new LoadMenuState(_services.Get<AssetProvider>());
			var menuState = new MenuState(_mediatorFactory, _services, _currencyMediator);
			var loadBattleState = new LoadBattleState(_services.Get<LoadingCurtain>(), _services.Get<AssetProvider>());
			var battleState = new BattleState(_services, _mediatorFactory, _services.Get<FlyItemService>(), _currencyMediator);
			var endBattleState = new EndBattleState(_mediatorFactory);

			_stateMachine.AddTransition(_loadMenuState, menuState, () => _loadMenuState.Complete);
			_stateMachine.AddTransition(menuState, loadBattleState, () => menuState.Complete);
			_stateMachine.AddTransition(loadBattleState, battleState, () => loadBattleState.Complete);
			_stateMachine.AddTransition(battleState,
			                            endBattleState,
			                            () =>
			                            {
				                            if (battleState.Complete)
					                            endBattleState.SetEndLevelResult(battleState.EndLevelMessage);
				                            return battleState.Complete;
			                            });
			_stateMachine.AddTransition(endBattleState, _loadMenuState, () => endBattleState.Complete);
			_stateMachine.SetState(_loadMenuState);
			_initialized = true;
		}

		private async Task InitAdditionalMediators()
		{
			await _mediatorFactory.Get<MainMenuMediator>();
			_currencyMediator = await _mediatorFactory.Get<CurrencyMediator>();
			await _mediatorFactory.Get<LootBoxMediator>();
			await _mediatorFactory.Show<CurrencyMediator>();
		}

		private void Update()
		{
			if (_initialized == false)
				return;

			_stateMachine.Tick();
		}

		private void OnDestroy()
		{
			_currencyMediator?.Dispose();
		}
	}
}