using GameFrame.Core.Collections;
using GameFrame.Core.Math;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Model
{
    public class WorldChunk
    {
        private Map<Int32, WorldTile> tileMap;

        public Vector2Int Position { get; set; }
        public Int32? DefaultTileHeight { get; set; }
        public List<WorldTile> Tiles { get; set; }

        public Map<Int32, WorldTile> GetTileMap()
        {
            if (tileMap == default)
            {
                tileMap = new Map<Int32, WorldTile>();

                if (Tiles?.Count > 0)
                {
                    foreach (var tile in Tiles)
                    {
                        tileMap[tile.Position.X, tile.Position.Z] = tile;
                    }
                }
            }

            return tileMap;
        }
    }
}
