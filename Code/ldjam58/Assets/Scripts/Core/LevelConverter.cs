using System;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core
{
    public class LevelConverter
    {
        public Level Convert(LevelDefinition levelDefinition)
        {
            if (!levelDefinition.PenguinStartPosition.HasValue && !levelDefinition.IsPenguinStartPositionRandom.HasValue)
            {
                throw new Exception("Missing LevelDefinition.PenguinStartingPosition");
            }

            var convertedLevel = new Level()
            {
                Name = levelDefinition.Name,
                Seed = levelDefinition.Seed,
                Size = levelDefinition.Size,
                Reference = levelDefinition.Reference,
                Description = levelDefinition.Description,
                Resolution = levelDefinition.Resolution,
                IsPenguinStartPositionRandom = levelDefinition.IsPenguinStartPositionRandom.GetValueOrDefault(),
                PenguinStartPosition = levelDefinition.PenguinStartPosition.GetValueOrDefault(),
                IsFoodPositionRandom = levelDefinition.IsFoodPositionRandom.GetValueOrDefault(),
                IsObstaclePositionRandom = levelDefinition.IsObstaclePositionRandom.GetValueOrDefault(),
            };

            if (levelDefinition.Foods?.Count > 0)
            {
                foreach (var foodDef in levelDefinition.Foods)
                {
                    convertedLevel.AvailableFoods.Add(ConvertFood(foodDef));
                }
            }

            if (levelDefinition.Obstacles?.Count > 0)
            {
                foreach (var obstacleDef in levelDefinition.Obstacles)
                {
                    convertedLevel.Obstacles.Add(ConvertObstacle(obstacleDef));
                }
            }

            if (levelDefinition.Chunks?.Count > 0)
            {
                foreach (var chunkDefinition in levelDefinition.Chunks)
                {
                    convertedLevel.Chunks.Add(ConvertChunk(chunkDefinition));
                }
            }

            return convertedLevel;
        }

        private Food ConvertFood(FoodPosDefinition foodDef)
        {
            return new Food()
            {
                ID = Guid.NewGuid(),
                Definition = foodDef.Definition,
                Position = new Vector3(foodDef.Position.X, 0, foodDef.Position.Y),
            };
        }

        private Obstacle ConvertObstacle(ObstaclePosDefinition obstacleDef)
        {
            return new Obstacle()
            {
                Definition = obstacleDef.ObstacleDefinition,
                Position = obstacleDef.Position
            };
        }

        private WorldChunk ConvertChunk(WorldChunkDefinition chunkDefinition)
        {
            var chunk = new WorldChunk()
            {
                Position = chunkDefinition.Position,
                DefaultTileHeight = chunkDefinition.DefaultTileHeight,
            };

            if (chunkDefinition.Tiles?.Count > 0)
            {
                foreach (var tileDefinition in chunkDefinition.Tiles)
                {
                    chunk.Tiles.Add(new WorldTile()
                    {
                        Position = tileDefinition
                    });
                }
            }

            return chunk;
        }
    }
}
