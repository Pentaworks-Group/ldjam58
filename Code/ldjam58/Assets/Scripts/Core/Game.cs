using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Definitons.Loaders;
using Assets.Scripts.Core.Persistence;
using GameFrame.Core.Definitions.Loaders;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.SaveableGame<GameState, PlayerOptions, SavedGamePreview>
    {
        private readonly DefinitionCache<Definitons.GameMode> gameModeCache = new DefinitionCache<Definitons.GameMode>();
        protected override GameState InitializeGameState()
        {
            var gameMode = default(Assets.Scripts.Core.Definitons.GameMode);

            if (gameMode == default)
            {
                gameMode = new Assets.Scripts.Core.Definitons.GameMode()
                {
                    Levels = new List<LevelDefinition>()
                    {
                        new LevelDefinition()
                        {
                            Name = "Test",
                            Description = "Test",
                            Reference = Guid.NewGuid().ToString(),
                            Size = new GameFrame.Core.Math.Vector2Int(16, 16),
                            Resolution = 16,
                            PenguinStartPosition = new GameFrame.Core.Math.Vector2Int(8, 7),
                            //Chunks = new List<WorldChunkDefinition>()
                            //{
                            //    new WorldChunkDefinition(1, 6, 4),
                            //    new WorldChunkDefinition(1, 7, 4),
                            //    new WorldChunkDefinition(1, 8, 4),
                            //    new WorldChunkDefinition(1, 9, 4),
                            //    new WorldChunkDefinition(1, 10, 4),
                            //    new WorldChunkDefinition(1, 6, 5),
                            //    new WorldChunkDefinition(1, 10, 5),
                            //    new WorldChunkDefinition(1, 5, 6),
                            //    new WorldChunkDefinition(1, 6, 6),
                            //    new WorldChunkDefinition(1, 7, 6),
                            //    new WorldChunkDefinition(1, 8, 6),
                            //    new WorldChunkDefinition(1, 9, 6),
                            //    new WorldChunkDefinition(1, 10, 6),
                            //    new WorldChunkDefinition(1, 11, 6),
                            //    new WorldChunkDefinition(1, 4, 7),
                            //    new WorldChunkDefinition(1, 5, 7),
                            //    new WorldChunkDefinition(1, 6, 7),
                            //    new WorldChunkDefinition(1, 7, 7),
                            //    new WorldChunkDefinition(1, 8, 7),
                            //    new WorldChunkDefinition(1, 9, 7),
                            //    new WorldChunkDefinition(1, 10, 7),
                            //    new WorldChunkDefinition(1, 11, 7),
                            //    new WorldChunkDefinition(1, 12, 7),
                            //    new WorldChunkDefinition(1, 4, 8),
                            //    new WorldChunkDefinition(1, 5, 8),
                            //    new WorldChunkDefinition(1, 6, 8),
                            //    new WorldChunkDefinition(1, 7, 8),
                            //    new WorldChunkDefinition(1, 8, 8),
                            //    new WorldChunkDefinition(1, 9, 8),
                            //    new WorldChunkDefinition(1, 10, 8),
                            //    new WorldChunkDefinition(1, 11, 8),
                            //    new WorldChunkDefinition(1, 12, 8),
                            //    new WorldChunkDefinition(1, 4, 9),
                            //    new WorldChunkDefinition(1, 5, 9),
                            //    new WorldChunkDefinition(1, 7, 9),
                            //    new WorldChunkDefinition(1, 8, 9),
                            //    new WorldChunkDefinition(1, 9, 9),
                            //    new WorldChunkDefinition(1, 11, 9),
                            //    new WorldChunkDefinition(1, 12, 9),
                            //    new WorldChunkDefinition(1, 12, 10),
                            //    new WorldChunkDefinition(1, 4, 10),
                            //    new WorldChunkDefinition(1, 5, 10),
                            //    new WorldChunkDefinition(1, 6, 10),
                            //    new WorldChunkDefinition(1, 7, 10),
                            //    new WorldChunkDefinition(1, 8, 10),
                            //    new WorldChunkDefinition(1, 9, 10),
                            //    new WorldChunkDefinition(1, 10, 10),
                            //    new WorldChunkDefinition(1, 11, 10),
                            //    new WorldChunkDefinition(1, 12, 10),
                            //    new WorldChunkDefinition(1, 5, 11),
                            //    new WorldChunkDefinition(1, 6, 11),
                            //    new WorldChunkDefinition(1, 7, 11),
                            //    new WorldChunkDefinition(1, 9, 11),
                            //    new WorldChunkDefinition(1, 10, 11),
                            //    new WorldChunkDefinition(1, 11, 11),
                            //}
                            Chunks = new List<WorldChunkDefinition>()
                            {
                                new WorldChunkDefinition(4, 6, 4),
                                new WorldChunkDefinition(4, 7, 4),
                                new WorldChunkDefinition(4, 8, 4),
                                new WorldChunkDefinition(4, 9, 4),
                                new WorldChunkDefinition(4, 10, 4),

                                new WorldChunkDefinition(4, 5, 5),
                                new WorldChunkDefinition(3, 6, 5),
                                new WorldChunkDefinition(3, 7, 5),
                                new WorldChunkDefinition(3, 8, 5),
                                new WorldChunkDefinition(3, 9, 5),
                                new WorldChunkDefinition(3, 10, 5),
                                new WorldChunkDefinition(4, 11, 5),

                                new WorldChunkDefinition(4, 4, 6),
                                new WorldChunkDefinition(3, 5, 6),
                                new WorldChunkDefinition(2, 6, 6),
                                new WorldChunkDefinition(1, 7, 6),
                                new WorldChunkDefinition(1, 8, 6),
                                new WorldChunkDefinition(1, 9, 6),
                                new WorldChunkDefinition(2, 10, 6),
                                new WorldChunkDefinition(3, 11, 6),
                                new WorldChunkDefinition(4, 12, 6),

                                new WorldChunkDefinition(4, 4, 7),
                                new WorldChunkDefinition(3, 5, 7),
                                new WorldChunkDefinition(2, 6, 7),
                                new WorldChunkDefinition(1, 7, 7),
                                new WorldChunkDefinition(1, 8, 7),
                                new WorldChunkDefinition(1, 9, 7),
                                new WorldChunkDefinition(2, 10, 7),
                                new WorldChunkDefinition(3, 11, 7),
                                new WorldChunkDefinition(4, 12, 7),

                                new WorldChunkDefinition(4, 4, 8),
                                new WorldChunkDefinition(3, 5, 8),
                                new WorldChunkDefinition(2, 6, 8),
                                new WorldChunkDefinition(1, 7, 8),
                                new WorldChunkDefinition(1, 8, 8),
                                new WorldChunkDefinition(1, 9, 8),
                                new WorldChunkDefinition(2, 10, 8),
                                new WorldChunkDefinition(3, 11, 8),
                                new WorldChunkDefinition(4, 12, 8),

                                new WorldChunkDefinition(4, 4, 9),
                                new WorldChunkDefinition(3, 5, 9),
                                new WorldChunkDefinition(2, 6, 9),
                                new WorldChunkDefinition(1, 7, 9),
                                new WorldChunkDefinition(1, 8, 9),
                                new WorldChunkDefinition(1, 9, 9),
                                new WorldChunkDefinition(2, 10, 9),
                                new WorldChunkDefinition(3, 11, 9),
                                new WorldChunkDefinition(4, 12, 9),

                                new WorldChunkDefinition(4, 4, 10),
                                new WorldChunkDefinition(3, 5, 10),
                                new WorldChunkDefinition(2, 6, 10),
                                new WorldChunkDefinition(1, 7, 10),
                                new WorldChunkDefinition(1, 8, 10),
                                new WorldChunkDefinition(1, 9, 10),
                                new WorldChunkDefinition(2, 10, 10),
                                new WorldChunkDefinition(3, 11, 10),
                                new WorldChunkDefinition(4, 12, 10),

                                new WorldChunkDefinition(4, 5, 11),
                                new WorldChunkDefinition(3, 6, 11),
                                new WorldChunkDefinition(3, 7, 11),
                                new WorldChunkDefinition(3, 8, 11),
                                new WorldChunkDefinition(3, 9, 11),
                                new WorldChunkDefinition(3, 10, 11),
                                new WorldChunkDefinition(4, 11, 11),

                                new WorldChunkDefinition(4, 6, 12),
                                new WorldChunkDefinition(4, 7, 12),
                                new WorldChunkDefinition(4, 8, 12),
                                new WorldChunkDefinition(4, 9, 12),
                                new WorldChunkDefinition(4, 10, 12),
                            }
                        }
                    }
                };
            }

            var gameStateConverter = new GameStateConverter(gameMode);

            var gameState = gameStateConverter.Convert();

            return gameState;
        }

        protected override PlayerOptions InitializePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.9f,
                AmbienceVolume = 0.1f,
                BackgroundVolume = 0.3f,
            };
        }

        protected override void RegisterScenes()
        {
            RegisterScenes(Constants.Scenes.GetAll());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }

        protected override IEnumerator LoadDefintions()
        {
            var foodCache = new DefinitionCache<FoodDefinition>();
            var obstacleCache = new DefinitionCache<ObstacleDefinition>();

            yield return new DefinitionLoader<FoodDefinition>(foodCache).LoadDefinitions("Foods.json");
            yield return new DefinitionLoader<ObstacleDefinition>(obstacleCache).LoadDefinitions("Obstacles.json");
            yield return new GameModeLoader(this.gameModeCache, obstacleCache, foodCache).LoadDefinitions("GameModes.json");
            Debug.Log("loaded definitions");
        }
    }
}
