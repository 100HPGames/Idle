using CodeBase.Components.Interactions;
using CodeBase.InputService;
using CodeBase.Systems;
using CodeBase.Systems.EnemyStateMachine;
using CodeBase.Systems.LevelsProgression;
using CodeBase.Systems.PlayerStateMachine;
using CodeBase.Systems.UtilsSystems;
using CodeBase.Systems.ViewSystems;
using CodeBase.Tools;
using CodeBase.Tools.SimpleMessenger;
using CodeBase.Tools.StateMachine;
using CodeBase.UI.FlyService;
using CodeBase.UI.Mediators;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using MoneyHandler;

namespace CodeBase.Infrastructure.GameStates
{
	public class BattleState : ISelfCompleteState
	{
		public bool Complete { get; private set; }
		public EndLevelMessage EndLevelMessage {get; private set;}
		
		private readonly CurrencyMediator _currencyMediator;
		private readonly MediatorFactory _mediatorFactory;
		private readonly FlyItemService _flyItemService;
		private readonly ServiceLocator _locator;
		private BattleMediator _battleMediator;
		private IProtoSystems _systems;
		private Messenger _messenger;
		private ProtoWorld _world;
		private bool _isActive;

		public BattleState(ServiceLocator locator, MediatorFactory mediatorFactory, FlyItemService flyItemService,
		                   CurrencyMediator currencyMediator)
		{
			_flyItemService = flyItemService;
			_mediatorFactory = mediatorFactory;
			_locator = locator;
			_isActive = false;
			_currencyMediator = currencyMediator;
		}

		public async void OnEnter()
		{
			Complete = false;
			_messenger = _locator.Get<Messenger>();
			_messenger.Sub<EndLevelMessage>(ReactOnLevelComplete); 
			_world = new ProtoWorld(new GameAspect());
			_systems = new ProtoSystems(_world);
			InitSystems();
			_battleMediator = await _mediatorFactory.Get<BattleMediator>();
			var mediator = await _mediatorFactory.Get<FinishLevelMediator>();
			mediator.Hide();
			await _mediatorFactory.Show<BattleMediator>();
			_isActive = true;
			_currencyMediator.EnterBattle();
		}

		private void ReactOnLevelComplete(EndLevelMessage obj)
		{
			EndLevelMessage = obj;
			Complete = true;
		}

		public void OnExit()
		{
			_messenger.Unsub<EndLevelMessage>(EndLevel);
			_systems?.Destroy();
			_systems = null;
			_world?.Destroy();
			_world = null;
			_battleMediator.Dispose();
			_isActive = false;
		}

		public void Tick()
		{
			if (_isActive == false)
				return;

			_systems?.Run();
		}

		private void EndLevel(EndLevelMessage obj) => Complete = true;

		private void InitSystems()
		{
			_systems.AddModule(new AutoInjectModule(true)) // Инъекция в поля систем.
			        .AddModule(new UnityModule())          // Интеграция основных unity-систем.
			        .AddSystem(new InitPlayerSystem())
			        //Run-системы
			        .AddModule(new PlayerStateModule())
			        .AddSystem(new SwitchStateSystem())
			        .AddSystem(new DamageTextMoveSystem())
			        .AddSystem(new WinSystem())
			        .AddSystem(new LooseSystem())
			        .AddSystem(new MessagesDelSystem())
			        // Регистрация сервисов
			        .AddService(_locator.Get<AssetProvider>())
			        .AddService(_locator.Get<StaticData>())
			        .AddService(_locator.Get<Messenger>())
			        .AddService(_locator.Get<TouchInputService>())
			        .AddService(_locator.Get<CurrencyService>())
			        .AddService(_locator.Get<PurchaseService>())
			        .AddService(_flyItemService)
			        .Init();
		}
	}
}