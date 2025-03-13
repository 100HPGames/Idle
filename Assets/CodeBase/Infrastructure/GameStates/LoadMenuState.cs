using CodeBase.Infrastructure.Services;
using CodeBase.Tools.StateMachine;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.Mediators;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadMenuState : ISelfCompleteState
    {
	    private readonly AssetProvider _assetProvider;

	    public bool Complete { get; private set; }

        public LoadMenuState(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async void OnEnter()
        {
            Complete = false;
            await _assetProvider.LoadSceneSingle(GameConst.MainScene);
            Complete = true;
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
          
        }
    }
}