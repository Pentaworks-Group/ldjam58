using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core
{
    public class GameStateConverter
    {
        private readonly GameMode mode;
        private IDictionary<String, Food> foodMap;
        private IDictionary<String, Obstacle> obstacleMap;

        public GameStateConverter(GameMode gameMode)
        {
            this.mode = gameMode;
        }

        public GameState Convert()
        {
            var gameState = new GameState()
            {
                CreatedOn = DateTime.Now,
                CurrentScene = Constants.Scenes.GameName,
                Mode = mode,
            };

            if (!this.mode.IsRandomGenerated)
            {
                if (this.mode.Levels?.Count > 0)
                {
                    var firstLevel = this.mode.Levels[0];

                    gameState.CurrentLevel = ConvertLevel(firstLevel);

                }
                else
                {
                    throw new Exception("Neither Levels provided nor Random flag set in GameMode!");
                }
            }

            return gameState;
        }

        private Level ConvertLevel(LevelDefinition levelDefinition)
        {
            var startingPosition = default(Vector2Int);

            if (levelDefinition.PenguinStartPosition.HasValue)
            {
                startingPosition = levelDefinition.PenguinStartPosition.Value;
            }
            else
            {
                throw new Exception("Missing LevelDefinition.PenguinStartingPosition");
            }

            var convertedLevel = new Level()
            {
                Size = levelDefinition.Size,
                PenguinStartPosition = startingPosition,
                Resolution = levelDefinition.Resolution,
                Name = levelDefinition.Name,
                Description = levelDefinition.Description,
                Seed = levelDefinition.Seed,
                Foods = new List<Food>(),
                Obstacles = new List<Obstacle>()
            };

            if (levelDefinition.Foods?.Count > 0)
            {
                foreach (var foodDef in levelDefinition.Foods)
                {
                    convertedLevel.Foods.Add(ConvertFood(foodDef));
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
                Definition = foodDef.Food,
                Position = foodDef.Position
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
