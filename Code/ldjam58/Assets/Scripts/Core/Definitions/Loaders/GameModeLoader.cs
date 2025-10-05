using GameFrame.Core.Definitions.Loaders;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons.Loaders
{
    public class GameModeLoader : BaseLoader<GameMode>
    {
        private readonly DefinitionCache<ObstacleDefinition> obstacleCache;
        private readonly DefinitionCache<FoodDefinition> foodCache;

        public GameModeLoader(DefinitionCache<GameMode> targetCache, DefinitionCache<ObstacleDefinition> obstacleCache, DefinitionCache<FoodDefinition> fooodCache) : base(targetCache)
        {
            this.obstacleCache = obstacleCache;
            this.foodCache = fooodCache;
        }

        protected override void OnDefinitionsLoaded(List<GameMode> definitions)
        {
            _ = new GameMode() { IsReferenced = true };

            if (definitions?.Count > 0)
            {
                foreach (var loadedGameMode in definitions)
                {
                    var newGameMode = new GameMode()
                    {
                        Reference = loadedGameMode.Reference,                       
                        Name = loadedGameMode.Name,
                        Description = loadedGameMode.Description,
                        AreLevelsRandom = loadedGameMode.AreLevelsRandom,
                        IsAllowingControlWhileMoving = loadedGameMode.IsAllowingControlWhileMoving,
                        Levels = new List<LevelDefinition>()
                    };

                    if (loadedGameMode.Levels != default)
                    {
                        CheckLevels(loadedGameMode.Levels, newGameMode.Levels);
                    }

                    if (loadedGameMode.Penguin != default)
                    {
                        newGameMode.Penguin = new PenguinDefinition()
                        {
                            Reference = loadedGameMode.Penguin.Reference,
                            Name = loadedGameMode.Penguin.Name,
                            Sprite = loadedGameMode.Penguin.Sprite,
                            Strength = loadedGameMode.Penguin.Strength,
                            MaxStrength = loadedGameMode.Penguin.MaxStrength
                        };
                    }

                    targetCache[loadedGameMode.Reference] = newGameMode;
                }
            }
        }
        private void CheckLevels(List<LevelDefinition> loadedItems, List<LevelDefinition> targetItems)
        {
            if (loadedItems?.Count > 0)
            {
                foreach (var loadedItem in loadedItems)
                {
                    var targetLevel = new LevelDefinition()
                    {
                        Reference = loadedItem.Reference,
                        Name = loadedItem.Name,
                        Description = loadedItem.Description,
                        Seed = loadedItem.Seed,
                        Size = loadedItem.Size,
                        Resolution = loadedItem.Resolution,
                        IsPenguinStartPositionRandom = loadedItem.IsPenguinStartPositionRandom,
                        ActiveFoodLimit = loadedItem.ActiveFoodLimit,
                        PenguinStartPosition = loadedItem.PenguinStartPosition,
                        FoodRandomOrder = loadedItem.FoodRandomOrder,
                        IsFoodPositionRandom = loadedItem.IsFoodPositionRandom,
                        ObstacleRandomOrder = loadedItem.ObstacleRandomOrder,
                        IsObstaclePositionRandom = loadedItem.IsObstaclePositionRandom,
                        Foods = new List<FoodPosDefinition>(),
                        Chunks = new List<WorldChunkDefinition>(),
                        Obstacles = new List<ObstaclePosDefinition>(),
                    };

                    CheckChunks(loadedItem.Chunks, targetLevel.Chunks);
                    CheckFoods(loadedItem.Foods, targetLevel.Foods);
                    CheckObstacles(loadedItem.Obstacles, targetLevel.Obstacles);                    

                    targetItems.Add(targetLevel);
                }
            }
        }

        private void CheckChunks(List<WorldChunkDefinition> loadedChunks, List<WorldChunkDefinition> targetChunks)
        {
            if (loadedChunks?.Count > 0)
            {
                targetChunks.AddRange(loadedChunks);
            }
        }

        private void CheckFoods(List<FoodPosDefinition> loadedFoods, List<FoodPosDefinition> targetFoods)
        {
            if (loadedFoods?.Count > 0)
            {
                foreach (var food in loadedFoods)
                {
                    var targetFood = new FoodPosDefinition()
                    {
                        Position = food.Position,
                    };

                    if (food.Definition != default)
                    {
                        targetFood.Definition = CheckItem(food.Definition, this.foodCache);
                    }

                    targetFoods.Add(targetFood);
                }
            }
        }

        private void CheckObstacles(List<ObstaclePosDefinition> loadedObstacles, List<ObstaclePosDefinition> targetObstacles)
        {
            if (loadedObstacles?.Count > 0)
            {
                foreach (var food in loadedObstacles)
                {
                    var targetFood = new ObstaclePosDefinition()
                    {
                        Position = food.Position,
                        ObstacleDefinition = CheckItem(food.ObstacleDefinition, this.obstacleCache)
                    };

                    targetObstacles.Add(targetFood);
                }
            }
        }
    }

}
