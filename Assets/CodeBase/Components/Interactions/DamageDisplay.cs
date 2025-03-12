using CodeBase.MonoBehaivours;
using UnityEngine;

namespace CodeBase.Components
{
    public struct DamageDisplay
    {
        public float Duration;
        public float Value;
        public Transform Source;
        public DamageView View;
        public float Transition;
        public Vector3 StartPosition;
    }
}