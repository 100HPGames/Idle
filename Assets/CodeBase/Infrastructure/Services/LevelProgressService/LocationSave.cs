using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure
{
    [Serializable]
    public class LocationSave
    {
        public CompletionState CompletionState;
        public List<LevelSave> LevelSaves;
    }
}