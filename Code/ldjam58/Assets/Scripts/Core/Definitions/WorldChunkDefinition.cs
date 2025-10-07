using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Definitions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Definitions
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
        public List<WorldTileDefinition> Tiles { get; set; }
    }
}
