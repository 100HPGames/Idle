using System.Collections.Generic;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;

namespace CodeBase.Components.States
{
    public struct NextState : IProtoAutoReset<NextState>
    {
        public List<StateType> States;
        public float ExternalDuration;
        
        public void AutoReset(ref NextState c)
        {
            c.States = new List<StateType>();
            c.ExternalDuration = -1;
        }
    }
}