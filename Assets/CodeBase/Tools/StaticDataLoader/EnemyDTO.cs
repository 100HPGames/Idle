using System;

namespace CodeBase.Tools.StaticDataLoader
{
    [Serializable]
    public class EnemyDTO
    {
        public EnemyId Id;
        public string Name;
        public string Description;
        public float Reward;
        public float Health;
        public float Damage;
        public float AttackCooldown;
        public float AttackDuration;
    }
}