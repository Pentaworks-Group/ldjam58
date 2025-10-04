using Assets.Scripts.Core.Model;
using GameFrame.Core.Definitions;
using GameFrame.Core.Math;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons
{
    public class WorldChunkDefinition : BaseDefinition
    {
        public WorldChunkDefinition() 
        { }

        public WorldChunkDefinition(Int32 defaultTileHeight, Int32 x, Int32 y)
        {
            this.DefaultTileHeight = defaultTileHeight;
            this.Position = new Vector2Int(x, y);
        }

        public Vector2Int Position { get; set; }
        public Int32? DefaultTileHeight { get; set; }
        public List<Vector3Int> Tiles { get; set; }

    }
}
