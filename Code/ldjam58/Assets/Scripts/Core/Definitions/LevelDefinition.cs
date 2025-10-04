using GameFrame.Core.Definitions;
using GameFrame.Core.Math;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons
{
    public class LevelDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Single? Seed { get; set; }
        public Int32? Width { get; set; }
        public Int32? Height { get; set; }
        public Vector2Int? PinguinStartPosition { get; set; }
        public Boolean? FoodRandomOrder { get; set; }
        public Boolean? FoodRandomPosition { get; set; }
        public Boolean? ObstacleRandomOrder{ get; set; }
        public Boolean? ObstacleRandomPosition { get; set; }
        public List<FoodPosDefinition> Foods { get; set; }
        public List<ObstaclePosDefinition> Obstacles { get; set; }
    }
}
