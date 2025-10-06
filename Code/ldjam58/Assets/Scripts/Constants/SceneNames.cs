using System;
using System.Collections.Generic;

namespace Assets.Scripts.Constants
{
    public class SceneNames
    {
        public const String MainMenu = Scenes.MainMenuName;
        public const String Options = Scenes.OptionsName;
        public const String SavedGames = Scenes.SavedGamesName;
        public const String Credits = Scenes.CreditsName;
        public const String Game = Scenes.GameName;
        public const String LevelCompleted = Scenes.LevelCompletedName;
        public const String GameOver = Scenes.GameOverName;
        public const String ShootingStars = Scenes.ShootingStarsName;

        public static List<String> GameSceneNames = new() { Scenes.MainMenuName, Scenes.OptionsName, Scenes.SavedGamesName, Scenes.CreditsName, Scenes.GameName, LevelCompleted, Scenes.GameOverName, Scenes.ShootingStarsName };
        public static List<String> DevelopmentSceneNames = new() { };
    }
}