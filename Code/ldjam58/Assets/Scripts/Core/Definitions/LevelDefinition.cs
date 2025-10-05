using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Definitons
{
    public class LevelDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Single? Seed { get; set; }
        public Vector2Int Size { get; set; }
        public Int32 Resolution { get; set; }
        public Boolean? IsPenguinStartPositionRandom { get; set; }
        public Vector2Int? PenguinStartPosition { get; set; }
        public Int32? ActiveFoodLimit { get; set; }
        public Boolean? FoodRandomOrder { get; set; }
        public Boolean? IsFoodPositionRandom { get; set; }
        public Boolean? ObstacleRandomOrder { get; set; }
        public Boolean? IsObstaclePositionRandom { get; set; }
        public List<FoodPosDefinition> Foods { get; set; }
        public List<WorldChunkDefinition> Chunks { get; set; }
        public List<ObstaclePosDefinition> Obstacles { get; set; }
    }
}
