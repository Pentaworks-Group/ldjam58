using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        private GameMode mode;
        public GameMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode != value)
                {
                    mode = value;
                }
            }
        }

        public Penguin Penguin { get; set; }
        public Level CurrentLevel { get; set; }

        public Double TimeElapsed { get; set; } = 0.0;
        public String DeathReason { get; set; }
        public Int32 FoodEaten { get; set; }
        public Int32 RemainingLives { get; set; }

        public void FillFoods()
        {
            var currentLevel = CurrentLevel;
            var levelDefinition = Mode.Levels.FirstOrDefault(l => l.Reference == currentLevel.Reference);

            if (levelDefinition.ActiveFoodLimit.HasValue && levelDefinition.ActiveFoodLimit.Value > 0)
            {
                if (currentLevel.AvailableFoods?.Count > 0)
                {
                    if (Mode.AreLevelsRandom)
                    {
                        // Not yet supported
                    }
                    else
                    {
                        if (currentLevel.Foods == default)
                        {
                            currentLevel.Foods = new List<Food>();
                        }

                        while (currentLevel.Foods.Count < levelDefinition.ActiveFoodLimit.Value)
                        {
                            var food = default(Food);

                            if (levelDefinition.FoodRandomOrder.HasValue)
                            {
                                if (levelDefinition.FoodRandomOrder.Value)
                                {
                                    food = currentLevel.AvailableFoods.GetRandomEntry();
                                }
                                else
                                {
                                    food = currentLevel.AvailableFoods.FirstOrDefault();
                                }

                                if (food != default)
                                {
                                    currentLevel.AvailableFoods.Remove(food);
                                    currentLevel.Foods.Add(food);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
