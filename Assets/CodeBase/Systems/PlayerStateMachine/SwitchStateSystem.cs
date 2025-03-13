using System.Collections.Generic;
using System.Linq;
using CodeBase.Components.States;
using CodeBase.Tools.SimpleMessenger;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Systems.PlayerStateMachine
{
    public class SwitchStateSystem : IProtoRunSystem
    {
        [DI] private GameAspect _aspect;
        [DI] private Messenger _messenger;
        
        public void Run()
        {
            foreach (var entity in _aspect.SwitchStatesIt)
            {
                ref var state = ref _aspect.CurrentStates.Get(entity);
                
                if (state.State == StateType.Dead)
                    continue;
                
                if (state.State == StateType.RunAway)
                    continue;
                
                ref var nextState = ref _aspect.NextStates.Get(entity);
                List<StateType> stateTypes = nextState.States.OrderByDescending(s => (int)s).ToList();
                TryAddInterruptedState(stateTypes, entity, state);
                state.State = stateTypes[0];
                if (nextState.ExternalDuration > 0) state.DurationValue = nextState.ExternalDuration;
                _aspect.NextStates.Del(entity);
            }
        }

        private void TryAddInterruptedState(List<StateType> stateTypes, ProtoEntity entity, CurrentState state)
        {
            if (stateTypes[0] == StateType.Disable && state.State != StateType.Disable)
            {
                ref var interrupted = ref _aspect.InterruptedStates.GetOrAdd(entity);
                interrupted.State = state.State;
            }
        }
    }
}