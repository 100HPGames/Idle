using System.Collections.Generic;
using CodeBase.Components.Interactions;
using CodeBase.Infrastructure.Services.MediatoerService;
using CodeBase.Tools.StateMachine;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.LootBoxes;
using CodeBase.UI.Mediators;

namespace CodeBase.Infrastructure.GameStates
{
	public class EndBattleState : ISelfCompleteState
	{
		private readonly MediatorFactory _mediatorFactory;
		private FinishLevelMediator _finishLevelMediator;
		private LootBoxMediator _lootBoxMediator;
		private EndLevelMessage _endLevelMessage;

		public bool Complete { get; private set; }

		public void SetEndLevelResult(EndLevelMessage endLevelMessage) => _endLevelMessage = endLevelMessage;

		public EndBattleState(MediatorFactory mediatorFactory)
		{
			_mediatorFactory = mediatorFactory;
		}

		public async void OnEnter()
		{
			Complete = false;
			_finishLevelMediator = await _mediatorFactory.Get<FinishLevelMediator>();
			_finishLevelMediator.OnClickEnd += EndState;
		
			int soft = _endLevelMessage.Soft;
			int hard = _endLevelMessage.Hard;
			
			if (_endLevelMessage.Win)
			{
				_finishLevelMediator.OpenWinView("locationName", soft, hard);
			}
			else
			{
				_finishLevelMediator.OpenLooseView("locationName", soft, hard);
			}
			
			_ = _mediatorFactory.Show<FinishLevelMediator>();
		}

		public void OnExit()
		{
			_endLevelMessage = default;
			_finishLevelMediator.OnClickEnd -= EndState;
		}

		public void Tick()
		{
		}

		private void EndState()
		{
			Complete = true;
		}
	}
}