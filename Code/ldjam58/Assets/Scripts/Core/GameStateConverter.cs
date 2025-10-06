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
                Penguin = ConvertPenguin(mode.Penguin)
            };

            if (!this.mode.AreLevelsRandom)
            {
                if (this.mode.Levels?.Count > 0)
                {
                    var firstLevel = this.mode.Levels[2];

                    gameState.CurrentLevel = new LevelConverter().Convert(firstLevel);

                }
                else
                {
                    throw new Exception("Neither Levels provided nor Random flag set in GameMode!");
                }
            }

            return gameState;
        }

        private Penguin ConvertPenguin(PenguinDefinition penguinDefinition)
        {
            if (mode.Penguin != default)
            {
                var penguin = new Penguin()
                {
                    Name = penguinDefinition.Name
                };
                
                if (!penguinDefinition.Strength.HasValue)
                {
                    throw new Exception("Missing PenguinDefinition.Strength!");
                }
                
                penguin.Strength = penguinDefinition.Strength.Value;
                
                if (!penguinDefinition.MaxStrength.HasValue)
                {
                    throw new Exception("Missing PenguinDefinition.MaxStrength!");
                }
                
                penguin.MaxStrength = penguinDefinition.MaxStrength.Value;

                return penguin;
            }

            return default;
        }
    }
}
