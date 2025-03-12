using Leopotam.EcsProto;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class PlayerStateModule : IProtoModule
    {
        public void Init(IProtoSystems systems)
        {
            systems
                .AddSystem(new PlayerStartAttackSystem())
                .AddSystem(new PlayerMakeAttackSystem())
                .AddSystem(new PlayerReloadAttackSystem())
                .AddSystem(new PlayerDeathSystem())
                .AddSystem(new PlayerMoveSystem())
                ;
        }

        public IProtoAspect[] Aspects()
        {
            return default;
        }

        public IProtoModule[] Modules()
        {
            return default;
        }
    }
}