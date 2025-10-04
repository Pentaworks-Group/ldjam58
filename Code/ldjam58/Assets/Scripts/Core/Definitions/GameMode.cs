using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : GameFrame.Core.Definitions.GameMode
    {
        public PinguinDefinition PinguinDefinition { get; set; }
        public List<LevelDefinition> Levels { get; set; }
    }
}
