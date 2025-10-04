using System;
using System.Collections.Generic;
using GameFrame.Core.Definitions.Loaders;

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

                    if (loadedGameMode.Pinguin != default)
                    {
                        newGameMode.Pinguin = new PinguinDefinition()
                        {
                            Name = loadedGameMode.Pinguin.Name,
                            Sprite = loadedGameMode.Pinguin.Sprite,
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
                var targetItem = new LevelDefinition()
                {
                    Seed = loadedItem.Seed,
                    Width = loadedItem.Width,
                    Height = loadedItem.Height,
                    PinguinStartPosition = loadedItem.PinguinStartPosition,
                    Foods = new List<FoodDefinition>(),
                    Obstacles = new List<ObstacleDefinition>()
                };

                CheckItems(loadedItem.Foods, targetItem.Foods, this.fooodCache);
                CheckItems(loadedItem.Obstacles, targetItem.Obstacles, this.obstacleCache);

                targetItems.Add(targetItem);
            }
        }
    }

}
