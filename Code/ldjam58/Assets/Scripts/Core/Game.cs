using GameFrame.Core.Definitions;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Definitons.Loaders;
using GameFrame.Core.Definitions.Loaders;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        private readonly DefinitionCache<GameMode> gameModeCache = new DefinitionCache<GameMode>();
        protected override GameState InitializeGameState()
        {
            var gameMode = default(GameMode);

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
