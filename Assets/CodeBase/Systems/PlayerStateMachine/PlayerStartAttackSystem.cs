using CodeBase.Components.States;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class PlayerStartAttackSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;
        [DI] private StaticData _staticData;

        public void Run()
        {
            foreach (var entity in _aspect.PlayerStatesIt)
            {
                ref var state = ref _aspect.CurrentStates.Get(entity);

                if (state.State != StateType.StartAttack)
                    continue;

                if (_aspect.StartAttack.Has(entity) == false)
                {
                    ref var newStartAttack = ref _aspect.StartAttack.Add(entity);
                    newStartAttack.Timer = _staticData.GameDTO.PlayerDto.AutoAttackAnimationTime;
                    continue;
                }

                ref var startAttack = ref _aspect.StartAttack.Get(entity);
                startAttack.Timer -= Time.deltaTime;

                if (startAttack.Timer <= 0f)
                {
                    _aspect.StartAttack.Del(entity);
                    ref var nextState = ref _aspect.NextStates.GetOrAdd(entity);
                    nextState.States.Add(StateType.MakeAttack);
                }
            }
        }
    }
}