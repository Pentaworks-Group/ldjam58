using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Model
{
    public class World
    {
        /// <summary>
        /// Chunks per axis
        /// </summary>
        public GameFrame.Core.Math.Vector2 Size { get; set; }
        /// <summary>
        /// Tiles per Chunk axis
        /// </summary>
        public Int32 Resolution { get; set; }

        public List<WorldChunk> Chunks { get; set; } = new List<WorldChunk>();
    }
}
