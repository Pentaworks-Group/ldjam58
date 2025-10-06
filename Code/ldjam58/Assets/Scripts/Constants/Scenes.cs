﻿using System;
using System.Collections.Generic;

using GameFrame.Core;

namespace Assets.Scripts.Constants
{
    public static class Scenes
    {
        public const String MainMenuName = "MainMenuScene";
        private static Scene mainMenu;
        public static Scene MainMenu
        {
            get
            {
                if (mainMenu == default)
                {
                    mainMenu = new Scene()
                    {
                        Name = MainMenuName
                    };
                }

                return mainMenu;
            }
        }

        public const String CreditsName = "CreditsScene";
        private static Scene credits;
        public static Scene Credits
        {
            get
            {
                if (credits == default)
                {
                    credits = new Scene()
                    {
                        Name = CreditsName,
                        //AmbienceClips = new List<String>()
                        //{
                        //},
                    };
                }

                return credits;
            }
        }

        public const String OptionsName = "OptionsScene";
        private static Scene options;
        public static Scene Options
        {
            get
            {
                if (options == default)
                {
                    options = new Scene()
                    {
                        Name = OptionsName
                    };
                }

                return options;
            }
        }

        public const String SavedGamesName = "SavedGamesScene";
        private static Scene savedGames;
        public static Scene SavedGames
        {
            get
            {
                if (savedGames == default)
                {
                    savedGames = new Scene()
                    {
                        Name = SavedGamesName
                    };
                }

                return savedGames;
            }
        }

        public const String GameName = "GameScene";
        private static Scene game;
        public static Scene Game
        {
            get
            {
                if (game == default)
                {
                    game = new Scene()
                    {
                        Name = GameName,
                        IsStoppingBackgroundMusic = true
                        //AmbienceClips = new List<String>()
                        //{
                        //    "WoodSound"
                        //},
                        //BackgroundClips = new List<String>()
                        //{
                        //    "Background"
                        //}
                    };
                }

                return game;
            }
        }

        public const String LevelCompletedName = "LevelCompletedScene";
        private static Scene levelCompleted;
        public static Scene LevelCompleted
        {
            get
            {
                if (levelCompleted == default)
                {
                    levelCompleted = new Scene()
                    {
                        Name = LevelCompletedName,
                        //AmbienceClips = new List<String>()
                        //{
                        //    "WoodSound"
                        //},
                        //BackgroundClips = new List<String>()
                        //{
                        //    "Background"
                        //}
                    };
                }

                return levelCompleted;
            }
        }

        public const String GameOverName = "GameOverScene";
        private static Scene gameOver;
        public static Scene GameOver
        {
            get
            {
                if (gameOver == default)
                {
                    gameOver = new Scene()
                    {
                        Name = GameOverName
                    };
                }

                return gameOver;
            }
        }

        public const String ShootingStarsName = "ShootingStarsScene";
        private static Scene shootingStars;
        public static Scene ShootingStars
        {
            get
            {
                if (shootingStars == default)
                {
                    shootingStars = new Scene()
                    {
                        Name = ShootingStarsName,
                        BackgroundClips = new List<String>()
                        {
                            "ShootingStars"
                        }
                    };
                }

                return shootingStars;
            }
        }

        public static IList<Scene> GetAll()
        {
            return new List<Scene>()
            {
                MainMenu,
                Credits,
                Options,
                SavedGames,
                Game,
                GameOver
            };
        }
    }
}
