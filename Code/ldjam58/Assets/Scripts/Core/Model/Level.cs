using GameFrame.Core.Math;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Model
{
    public class Level
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Single? Seed { get; set; }
        /// <summary>
        /// Chunks per axis
        /// </summary>
        public GameFrame.Core.Math.Vector2Int Size { get; set; }
        /// <summary>
        /// Tiles per Chunk axis
        /// </summary>
        public Int32 Resolution { get; set; }
        public Vector2? PinguinStartPosition { get; set; }
        public List<Food> Foods { get; set; } = new List<Food>();
        public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();

        public List<WorldChunk> Chunks { get; set; } = new List<WorldChunk>();
    }
}
