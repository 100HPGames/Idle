using CodeBase.Infrastructure;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class PlayerMakeAttackSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;
        [DI] private StaticData _staticData;

        public void Run()
        {
            foreach (var entity in _aspect.PlayerStatesIt)
            {
                ref var state = ref _aspect.CurrentStates.Get(entity);

                if (state.State != StateType.MakeAttack)
                    continue;

                if (_aspect.MakeAttack.Has(entity) == false)
                {
                    ref var newMakeAttack = ref _aspect.MakeAttack.Add(entity);
                    newMakeAttack.Timer = _staticData.GameDTO.PlayerDto.AutoAttackAnimationTime;
                    continue;
                }

                ref var makeAttack = ref _aspect.MakeAttack.Get(entity);
                makeAttack.Timer -= Time.deltaTime;

                if (makeAttack.Timer <= 0f)
                {
                    _aspect.MakeAttack.Del(entity);
                    ref var nextState = ref _aspect.NextStates.GetOrAdd(entity);
                    nextState.States.Add(StateType.Reload);
                }
            }
        }
    }
}