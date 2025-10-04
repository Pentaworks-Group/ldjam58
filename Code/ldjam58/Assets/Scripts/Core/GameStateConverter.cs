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
                Levels= ConvertLevels()
            };

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
                Height = level.Height,
                Width = level.Width,
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
    }
}
