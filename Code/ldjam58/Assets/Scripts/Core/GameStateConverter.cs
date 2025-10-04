using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;
using System;
using System.Collections.Generic;


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
            var gameState = new GameState
            {
                CreatedOn = DateTime.Now,
                CurrentScene = Constants.Scenes.GameName,
                Mode = mode,
            };

            gameState.CurrentLevel = GenerateWorld();

            return gameState;
        }

        private List<Level> ConvertLevels()
        {
            var levelList = new List<Level>();

            foreach (var level in mode.Levels)
            {
                var convertedLevel = ConvertLevel(level);
                levelList.Add(convertedLevel);
            }

            return levelList;
        }

        private Level ConvertLevel(LevelDefinition level)
        {
            var convertedLevel = new Level()
            {
                Size = level.Size,
                PinguinStartPosition = level.PinguinStartPosition,
                Resolution = level.Resolution,
                Name = level.Name,
                Description = level.Description,
                Seed = level.Seed,
                Foods = new List<Food>(),
                Obstacles = new List<Obstacle>()
            };

            foreach (var foodDef in level.Foods)
            {
                convertedLevel.Foods.Add(ConvertFood(foodDef));
            }

            foreach (var obstacleDef in level.Obstacles)
            {
                convertedLevel.Obstacles.Add(ConvertObstacle(obstacleDef));
            }


            return convertedLevel;
        }

        private Food ConvertFood(FoodPosDefinition foodDef)
        {
            return new Food()
            {
                Definition = foodDef.FoodDefinition,
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

        private Level GenerateWorld()
        {
            var world = new Level()
            {
                Size = new GameFrame.Core.Math.Vector2Int(16, 16),
                Resolution = 16,
                Chunks = new System.Collections.Generic.List<WorldChunk>()
                {
                    new WorldChunk()
                    {
                        DefaultTileHeight = 1,
                        Position = new GameFrame.Core.Math.Vector2Int(2, 2),
                        Tiles = new System.Collections.Generic.List<WorldTile>()
                        {
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(5, 2, 5)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(5, 2, 6)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(6, 2, 6)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(6, 2, 5)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(1, 3, 1)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(9, 9, 9)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(1, 3, 9)
                            }
                        }
                    }

                }
            };

            return world;
        }
    }
}
