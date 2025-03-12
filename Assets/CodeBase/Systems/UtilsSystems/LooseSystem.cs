using CodeBase.Components.Interactions;
using CodeBase.Infrastructure;
using CodeBase.Tools.SimpleMessenger;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Systems.UtilsSystems
{
    public class LooseSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;
        [DI] private Messenger _messenger;

        public void Run()
        {
            foreach (var entity in _aspect.LooseIt)
            {
                _messenger.Pub(new EndLevelMessage() {Win = true, Soft = 100, Hard = 100});
                _aspect.LooseGame.Del(entity);
                _aspect.World().DelEntity(_aspect.PlayersIt.First().Entity);
            }
        }
    }
}