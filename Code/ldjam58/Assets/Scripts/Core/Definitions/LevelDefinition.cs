using GameFrame.Core.Math;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons
{
    public class LevelDefinition
    {
        public Single? Seed { get; set; }
        public Int32? Width { get; set; }
        public Int32? Height { get; set; }
        public Vector2? PinguinStartPosition { get; set; }
        public List<FoodDefinition> Foods { get; set; }
        public List<ObstacleDefinition> Obstacles { get; set; }
    }
}
