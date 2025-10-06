using System;
using System.Collections.Generic;

using GameFrame.Core.Collections;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Model
{
    public class Level
    {
        private Map<Int32, WorldChunk> chunkMap;

        public String Reference { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public Single? Seed { get; set; }
        public GameFrame.Core.Math.Vector2Int Size { get; set; }
        public Int32 Resolution { get; set; }

        public Boolean IsPenguinStartPositionRandom { get; set; }
        public Vector2Int PenguinStartPosition { get; set; }

        public Boolean IsFoodPositionRandom { get; set; }
        public List<Food> Foods { get; set; } = new List<Food>();
        public List<Food> AvailableFoods { get; set; } = new List<Food>();

        public Boolean IsObstaclePositionRandom { get; set; }
        public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();

        public List<WorldChunk> Chunks { get; set; } = new List<WorldChunk>();

        public Map<Int32, WorldChunk> GetChunkMap()
        {
            if (chunkMap == default)
            {
                chunkMap = new Map<Int32, WorldChunk>();

                if (Chunks?.Count > 0)
                {
                    foreach (var chunk in Chunks)
                    {
                        chunkMap[chunk.Position.X, chunk.Position.Y] = chunk;
                    }
                }
            }

            return chunkMap;
        }

        public void AddChunk(WorldChunk chunk)
        {
            if (Chunks == null)
            {
                Chunks = new List<WorldChunk>();
            }

            Chunks.Add(chunk);
            chunkMap[chunk.Position.X, chunk.Position.Y] = chunk;
        }

        public void RemoveChunk(WorldChunk chunk)
        {
            Chunks.Remove(chunk);
            chunkMap.Remove(chunk.Position.X, chunk.Position.Y);
        }
    }
}
