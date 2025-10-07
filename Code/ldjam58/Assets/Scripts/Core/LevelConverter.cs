using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Collections;
using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core
{
    public class LevelConverter
    {
        private readonly Map<Int32, WorldChunk> chunkMap = new Map<int, WorldChunk>();
        private IList<WorldChunk> chunkList;

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
                MovementLimit = levelDefinition.MovementLimit,
                IsPenguinStartPositionRandom = levelDefinition.IsPenguinStartPositionRandom.GetValueOrDefault(),
                PenguinStartPosition = levelDefinition.PenguinStartPosition.GetValueOrDefault(),
                IsFoodPositionRandom = levelDefinition.IsFoodPositionRandom.GetValueOrDefault(),
                IsFoodPositionRandomOnRetry = levelDefinition.IsFoodPositionRandomOnRetry.GetValueOrDefault(),
                IsObstaclePositionRandom = levelDefinition.IsObstaclePositionRandom.GetValueOrDefault(),
            };

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


            if (levelDefinition.Foods?.Count > 0)
            {
                foreach (var foodDef in levelDefinition.Foods)
                {
                    convertedLevel.AvailableFoods.Add(ConvertFood(foodDef, convertedLevel));
                }
            }

            return convertedLevel;
        }

        private Food ConvertFood(FoodPosDefinition foodDef, Level level)
        {
            var food = new Food()
            {
                ID = Guid.NewGuid(),
                Definition = foodDef.Definition,
                Score = foodDef.Definition.Score.GetValueOrDefault(1)
            };

            if (level.IsFoodPositionRandom)
            {
                if (!level.IsFoodPositionRandomOnRetry)
                {
                    food.Position = GetRandomPosition();
                }
            }
            else
            {
                food.Position = new GameFrame.Core.Math.Vector3(foodDef.Position.X, 0, foodDef.Position.Y);
            }

            return food;
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

            chunkMap[chunkDefinition.Position.X, chunkDefinition.Position.Y] = chunk;

            if (chunkDefinition.Tiles?.Count > 0)
            {
                chunk.Tiles = new List<WorldTile>();

                foreach (var tileDefinition in chunkDefinition.Tiles)
                {
                    if (tileDefinition.Position.HasValue && tileDefinition.Position.Value != default)
                    {
                        chunk.Tiles.Add(new WorldTile()
                        {
                            Position = tileDefinition.Position.Value
                        });
                    }
                }
            }

            return chunk;
        }

        private GameFrame.Core.Math.Vector3 GetRandomPosition()
        {
            if (chunkList == default)
            {
                chunkList = chunkMap.GetAll().ToList();
            }

            var chunk = chunkList.GetRandomEntry();

            chunkList.Remove(chunk);

            return new GameFrame.Core.Math.Vector3(chunk.Position.X, 0, chunk.Position.Y) ;
        }
    }
}
