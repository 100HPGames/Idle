using System.Collections.Generic;
using CodeBase.Tools;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.StateMachine;
using CodeBase.UI.Mediators;
using CodeBase.UI.NotificationFolder;
using MoneyHandler;
using UI.MainMenu;

namespace CodeBase.Infrastructure.GameStates
{
	public class MenuState : ISelfCompleteState
	{
		public bool Complete { get; private set; }
		
		private readonly LoadingCurtain _loadingCurtain;
		private readonly MediatorFactory _mediatorFactory;
		private readonly CurrencyMediator _currencyMediator;
		private readonly List<INotificationUsed> _mediators = new();
		
		private ShopMediator _shopMediator;
		private MainMenuMediator _mainMenuMediator;

		public MenuState(MediatorFactory mediatorFactory,
		                 ServiceLocator serviceLocator,
		                 CurrencyMediator currencyMediator)
		{
			_loadingCurtain = serviceLocator.Get<LoadingCurtain>();
			_mediatorFactory = mediatorFactory;
			_currencyMediator = currencyMediator;
		}

		public async void OnEnter()
		{
			Complete = false;
			_mediators.Clear();
			_mainMenuMediator = await _mediatorFactory.Get<MainMenuMediator>();
			_shopMediator = await _mediatorFactory.Get<ShopMediator>();
			_mediators.TryAdd(_shopMediator);
			await _mediatorFactory.Show<MainMenuMediator>();
			_mainMenuMediator.OnClickShrinkButton += ClickShrinkButton;
			_mediators.ForEach(x => x.OnUpdateNotification += UpdateNotificationShrinkButton);
			_loadingCurtain.Hide();
			_currencyMediator.ExitBattle();
		}
		
		private void CompleteMenuState()
		{
			Complete = true;
		}
		
		public void OnExit()
		{
			_mainMenuMediator.OnClickShrinkButton -= ClickShrinkButton;
			_mediators.ForEach(x => x.OnUpdateNotification -= UpdateNotificationShrinkButton);
		}

		private void ClickShrinkButton(WindowType windowType)
		{
			switch (windowType)
			{
				case WindowType.Shop:
					ShowShopMediator();
					break;
			}
		}

		private void UpdateNotificationShrinkButton(WindowType windowType, bool show)
			=> _mainMenuMediator.UpdateNotificationShrinkButton(windowType, show);

		private async void ShowShopMediator() => await _mediatorFactory.Show<ShopMediator>();

		public void Tick()
		{
		}
	}
}