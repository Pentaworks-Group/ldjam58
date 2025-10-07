using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class GameMode : GameFrame.Core.Definitions.GameMode
    {
        public Int32? AvailableLives { get; set; }
        public Boolean AreLevelsRandom { get; set; }
        public Boolean IsAllowingControlWhileMoving { get; set; }
        public PenguinDefinition Penguin { get; set; }
        public List<LevelDefinition> Levels { get; set; }
    }
}
