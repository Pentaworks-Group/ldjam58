using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Definitions.Loaders;
using Assets.Scripts.Core.Persistence;
using GameFrame.Core.Definitions.Loaders;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.SaveableGame<GameState, PlayerOptions, SavedGamePreview>
    {
        private readonly DefinitionCache<Definitions.GameMode> gameModeCache = new DefinitionCache<Definitions.GameMode>();

        private GameMode selectedGameMode;

        public IList<Definitions.GameMode> GetAvailableGameModes()
        {
            return this.gameModeCache.Values.ToList();
        }

        public void Start(GameMode gameMode)
        {
            this.selectedGameMode = gameMode;
            Start();
        }

        protected override GameState InitializeGameState()
        {
            var gameMode = selectedGameMode;
                        
            if (gameMode == default)
            {
                gameMode = gameModeCache.Values.FirstOrDefault();
            }

            if (gameMode == default)
            {
                throw new Exception("What happend? No GameMode!!");
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
