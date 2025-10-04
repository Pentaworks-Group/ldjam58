using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : GameFrame.Core.Definitions.GameMode
    {
        public PenguinDefinition PinguinDefinition { get; set; }
        public List<LevelDefinition> Levels { get; set; }
    }
}
