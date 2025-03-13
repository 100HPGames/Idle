using CodeBase.Components.States;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class PlayerDeathSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;

        public void Run()
        {
            foreach (var entity in _aspect.PlayerStatesIt)
            {
                ref var state = ref _aspect.CurrentStates.Get(entity);

                if (state.State != StateType.Dead)
                    continue;

                if (_aspect.Death.Has(entity) == false)
                {
                    ref var newDeath = ref _aspect.Death.Add(entity);
                    newDeath.Timer = GameConst.DeathTimer;
                    continue;
                }
                
                ref var death = ref _aspect.Death.Get(entity);
                death.Timer -= Time.deltaTime;
                
                if (death.Timer < 0)
                {
                    _aspect.LooseGame.NewEntity();
                }
            }
        }
    }
}