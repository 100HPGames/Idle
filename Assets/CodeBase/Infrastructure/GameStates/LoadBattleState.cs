using CodeBase.Tools.StateMachine;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.Mediators;

namespace CodeBase.Infrastructure.GameStates
{
	public class LoadBattleState : ISelfCompleteState
	{
		public bool Complete { get; private set; }
		private readonly LoadingCurtain _loadingCurtain;
		private readonly AssetProvider _assetProvider;

		public LoadBattleState(LoadingCurtain loadingCurtain, AssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
			_loadingCurtain = loadingCurtain;
		}

		public async void OnEnter()
		{
			_loadingCurtain.Show();
			Complete = true;
		}

		public void OnExit()
		{
			_loadingCurtain.Hide();
		}

		public void Tick()
		{
		}
	}
}