using GameFrame.Core.Definitions.Loaders;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitons.Loaders
{
    public class GameModeLoader : BaseLoader<GameMode>
    {
        private readonly DefinitionCache<ObstacleDefinition> obstacleCache;
        private readonly DefinitionCache<FoodDefinition> fooodCache;

        public GameModeLoader(DefinitionCache<GameMode> targetCache, DefinitionCache<ObstacleDefinition> obstacleCache, DefinitionCache<FoodDefinition> fooodCache) : base(targetCache)
        {
            this.obstacleCache = obstacleCache;
            this.fooodCache = fooodCache;
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
                        Levels = new List<LevelDefinition>()
                    };


                    if (loadedGameMode.Levels != default)
                    {
                        CheckLevels(loadedGameMode.Levels, newGameMode.Levels);
                    }

                    if (loadedGameMode.PinguinDefinition != default)
                    {
                        newGameMode.PinguinDefinition = new PinguinDefinition()
                        {
                            Reference = loadedGameMode.PinguinDefinition.Reference,
                            Name = loadedGameMode.PinguinDefinition.Name,
                            Sprite = loadedGameMode.PinguinDefinition.Sprite,
                        };
                    }

                    targetCache[loadedGameMode.Reference] = newGameMode;
                }
            }
        }
        private void CheckLevels(List<LevelDefinition> loadedItems, List<LevelDefinition> targetItems)
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
                    PinguinStartPosition = loadedItem.PinguinStartPosition,
                    FoodRandomOrder = loadedItem.FoodRandomOrder,
                    FoodRandomPosition = loadedItem.FoodRandomPosition,
                    Foods = new List<FoodPosDefinition>(),
                    ObstacleRandomOrder = loadedItem.ObstacleRandomOrder,
                    ObstacleRandomPosition = loadedItem.ObstacleRandomPosition,
                    Obstacles = new List<ObstaclePosDefinition>()
                };

                CheckFoods(loadedItem.Foods, targetLevel.Foods);
                CheckObstacles(loadedItem.Obstacles, targetLevel.Obstacles);

                targetItems.Add(targetLevel);
            }
        }

        private void CheckFoods(List<FoodPosDefinition> loadedFoodPos, List<FoodPosDefinition> targetFoodPos)
        {
            foreach (var food in loadedFoodPos)
            {
                var targetFood = new FoodPosDefinition()
                {
                    Position = food.Position,
                    FoodDefinition = CheckItem(food.FoodDefinition, this.fooodCache)
                };
                targetFoodPos.Add(targetFood);
            }
        }

        private void CheckObstacles(List<ObstaclePosDefinition> loadedFoodPos, List<ObstaclePosDefinition> targetFoodPos)
        {
            foreach (var food in loadedFoodPos)
            {
                var targetFood = new ObstaclePosDefinition()
                {
                    Position = food.Position,
                    ObstacleDefinition = CheckItem(food.ObstacleDefinition, this.obstacleCache)
                };
                targetFoodPos.Add(targetFood);
            }
        }
    }

}
