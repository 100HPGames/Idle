using CodeBase.Components.States;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class PlayerMoveSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;

        public void Run()
        {
            foreach (var entity in _aspect.PlayerStatesIt)
            {
                ref var state = ref _aspect.CurrentStates.Get(entity);

                if (state.State == StateType.Move && _aspect.StartMove.Has(entity) == false)
                {
                    _aspect.StartMove.Add(entity);
                    ref var playerView = ref _aspect.CharacterViews.Get(_aspect.PlayersIt.First().Entity);
                   // playerView.View.PlayRun();
                }

                if (state.State == StateType.Idle)
                {
                    if (_aspect.StartMove.Has(entity))
                    {
                        ref var playerView = ref _aspect.CharacterViews.Get(entity);
                       // playerView.View.StopRun();
                        _aspect.StartMove.Del(entity);
                    }
                }
            }
        }
    }
}