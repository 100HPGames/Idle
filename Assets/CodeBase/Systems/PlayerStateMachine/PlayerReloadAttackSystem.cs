using CodeBase.Infrastructure;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class PlayerReloadAttackSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;
        [DI] private StaticData _staticData;

        public void Run()
        {
            foreach (var entity in _aspect.PlayerStatesIt)
            {
                ref var state = ref _aspect.CurrentStates.Get(entity);

                if (state.State != StateType.Reload)
                    continue;

                if (_aspect.ReloadAttack.Has(entity) == false)
                {
                    ref var newReloadAttack = ref _aspect.ReloadAttack.Add(entity);
                    var reloadDuration = _staticData.GameDTO.PlayerDto.AutoAttackReloadTime ;
                    newReloadAttack.Timer = reloadDuration;
                    continue;
                }

                ref var reloadAttack = ref _aspect.ReloadAttack.Get(entity);
                reloadAttack.Timer -= Time.deltaTime;

                if (reloadAttack.Timer <= 0f)
                {
                    _aspect.ReloadAttack.Del(entity);
                    ref var nextState = ref _aspect.NextStates.GetOrAdd(entity);
                    nextState.States.Add(_aspect.AliveEnemiesIt.IsEmpty() ? StateType.Idle : StateType.StartAttack);
                }
            }
        }
    }
}