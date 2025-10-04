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
        /// <summary>
        /// Chunks per axis
        /// </summary>
        public Vector2Int Size { get; set; }
        /// <summary>
        /// Tiles per Chunk axis
        /// </summary>
        public Int32 Resolution { get; set; }

        public List<WorldChunkDefinition> Chunks { get; set; } = new List<WorldChunkDefinition>();
        public Vector2Int? PenguinStartPosition { get; set; }
        public Boolean? FoodRandomOrder { get; set; }
        public Boolean? FoodRandomPosition { get; set; }
        public Boolean? ObstacleRandomOrder { get; set; }
        public Boolean? ObstacleRandomPosition { get; set; }
        public List<FoodPosDefinition> Foods { get; set; }
        public List<ObstaclePosDefinition> Obstacles { get; set; }
    }
}
