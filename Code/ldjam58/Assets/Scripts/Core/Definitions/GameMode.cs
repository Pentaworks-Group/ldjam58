using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : BaseDefinition
    {
        public String Name { get; set; }
        public PinguinDefinition Pinguin { get; set; }
        public List<LevelDefinition> Levels { get; set; }
    }
}
