namespace CodeBase.Stats
{
    public class SimpleStat
    {
        public StatType Type;
        public float Value;
    }
    
    public enum StatType
    {
        None = 0,
        Duration = 10,
        TickTime = 11,
        Damage = 20,
        Amount = 100,
    }
}