using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : GameFrame.Core.Definitions.GameMode
    {
        public Boolean IsRandomGenerated { get; set; }
        public PenguinDefinition Penguin { get; set; }
        public List<LevelDefinition> Levels { get; set; }
    }
}
