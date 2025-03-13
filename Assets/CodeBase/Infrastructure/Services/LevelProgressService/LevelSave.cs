using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.LevelProgressService
{
    [Serializable]
    public class LevelSave
    {
        public CompletionState CompletionState;
        public Difficulty CompleteDifficulty;
        public Dictionary<Difficulty, int> DifficultyProgress;
        public Dictionary<int, RewardState> RewardStates;
    }

    public enum RewardState
    {
        Locked = 0,
        Available = 1,
        Collected = 2,
    }
}