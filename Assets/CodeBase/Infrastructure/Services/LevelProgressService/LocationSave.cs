using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.LevelProgressService
{
    [Serializable]
    public class LocationSave
    {
        public CompletionState CompletionState;
        public List<LevelSave> LevelSaves;
    }
}