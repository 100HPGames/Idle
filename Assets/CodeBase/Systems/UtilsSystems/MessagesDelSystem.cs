using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Systems.UtilsSystems
{
    public class MessagesDelSystem : IProtoRunSystem
    {
        [DI] private GameAspect _gameAspect;
        
        public void Run()
        {
            foreach (var entity in _gameAspect.PlayerInputsIt)
                _gameAspect.PlayerInputs.Del(entity);
        }
    }
}