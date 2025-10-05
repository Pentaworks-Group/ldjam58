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

                    gameState.CurrentLevel = new LevelConverter().Convert(firstLevel);

                }
                else
                {
                    throw new Exception("Neither Levels provided nor Random flag set in GameMode!");
                }
            }

            return gameState;
        }
    }
}
