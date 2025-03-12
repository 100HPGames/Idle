namespace CodeBase.Tools.StaticDataLoader
{
    public static class GameConst
    {
        // PARSE STATS
        public const string BaseStat = "BaseStat";
        public const string DividerTag = "-->";
        public const string Key = "KEY";
        public const string Owner = "Owner";
        public const string GameDataFileName = "GameData";
        public const string GameDataFilePath = "GameData/GameData";
        public const string ListIndex = "ListIndex_";
        public const string Empty = "EMPTY";
        public const string PlayerDTO = "https://docs.google.com/spreadsheets/d/10KX0IT0j7HrJTEvMoE3qzFjt5h0to5qNwWE35UUWae4/edit?gid=0#gid=0";
        public const string ShopDTO = "https://docs.google.com/spreadsheets/d/10KX0IT0j7HrJTEvMoE3qzFjt5h0to5qNwWE35UUWae4/edit?gid=1723652180#gid=1723652180";
        public const string SpellsDTO = "https://docs.google.com/spreadsheets/d/10KX0IT0j7HrJTEvMoE3qzFjt5h0to5qNwWE35UUWae4/edit?gid=1590419892#gid=1590419892";
        public const string EnemiesDTO = "https://docs.google.com/spreadsheets/d/10KX0IT0j7HrJTEvMoE3qzFjt5h0to5qNwWE35UUWae4/edit?gid=1707607232#gid=1707607232";
        public const string LevelsDTO = "https://docs.google.com/spreadsheets/d/10KX0IT0j7HrJTEvMoE3qzFjt5h0to5qNwWE35UUWae4/edit?gid=864688622#gid=864688622";
        
        // STATIC DATA STATS
        public const string PersistentKey = "PersistentDataSaveKey";
        public const string MainScene = "MainScene";
        public const string BattleScene = "BattleScene";
        public const string DummyScene = "DummyScene";
        public const string MakeAttack = "MakeAttack";
        
        // GAME DEFAULTS
        public const float DistanceThreshold = 0.5f;
        public const float ProjectileDeviation = 0.1f;
        public const float BombDeviation = 4f;
        public const float PlayerProjectileFlyTime = 0.25f;
        public const float PlayerCastTime = 0.5f;
        public const float EnemyProjectileFlyTime = 0.65f;
        public const float DeathTimer = 2.5f;
        public const float PlayerAttackTime = 0.3f;
        public const float DamageViewDistance = 2f;
        public const float DefaultStunDuration = 3.0f;
        public const float SpawnerDelay = 1.5f;
        public const float ShowSelectWindowTimer = 10f;
    }
}