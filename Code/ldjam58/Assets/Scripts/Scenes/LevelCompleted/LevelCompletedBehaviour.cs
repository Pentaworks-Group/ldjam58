using System;
using System.Linq;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitons;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.LevelCompleted
{
    public class LevelCompletedBehaviour : MonoBehaviour
    {
        private GameState gameState;
        private LevelDefinition nextLevelDefinition;

        public Button nextLevelButton;
        public Button quitButton;

        public TMP_Text levelNameText;
        public TMP_Text modeCompletedText;
        public TMP_Text nextLevelText;

        public void OnNextLevelClicked()
        {
            gameState.CurrentLevel = new LevelConverter().Convert(nextLevelDefinition);
            Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.Game);
        }

        public void OnToMainMenuButtonClicked()
        {
            Base.Core.Game.Stop();
            Assets.Scripts.Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.MainMenu);
        }

        private void OnGameInitialized()
        {
            this.gameState = Base.Core.Game.State;

            levelNameText.text = String.Format("You have managed to completed the level '{0}'.", gameState.CurrentLevel.Name);

            var currentLevelDefinition = gameState.Mode.Levels.FirstOrDefault(l => l.Reference == gameState.CurrentLevel.Reference);

            var currentLevelIndex = gameState.Mode.Levels.IndexOf(currentLevelDefinition);

            if (currentLevelIndex < gameState.Mode.Levels.Count-1)
            {
                this.nextLevelDefinition = gameState.Mode.Levels[currentLevelIndex + 1];

                nextLevelText.text = String.Format("Give '{0}' a try.", nextLevelDefinition.Name);
                nextLevelText.gameObject.SetActive(true);
                nextLevelButton.gameObject.SetActive(true);
            }
            else
            {
                modeCompletedText.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
            }
        }

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(OnGameInitialized);
        }
    }
}
