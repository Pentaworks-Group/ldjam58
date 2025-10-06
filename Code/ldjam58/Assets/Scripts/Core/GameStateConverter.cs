using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;

namespace Assets.Scripts.Core
{
    public class GameStateConverter
    {
        private readonly GameMode mode;

        public GameStateConverter(GameMode gameMode)
        {
            this.mode = gameMode;
        }

        public GameState Convert()
        {
            if (!mode.AvailableLives.HasValue)
            {
                throw new Exception("GameMode.AvailableLives is Required!");
            }

            var gameState = new GameState()
            {
                CreatedOn = DateTime.Now,
                CurrentScene = Constants.Scenes.GameName,
                Mode = mode,
                RemainingLives = mode.AvailableLives.Value,
                Penguin = ConvertPenguin(mode.Penguin)
            };

            if (!this.mode.AreLevelsRandom)
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

        private Penguin ConvertPenguin(PenguinDefinition penguinDefinition)
        {
            if (mode.Penguin != default)
            {
                var penguin = new Penguin()
                {
                    Name = penguinDefinition.Name
                };

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
