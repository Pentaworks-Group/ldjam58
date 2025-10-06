using System;

using Assets.Scripts.Core;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameOver
{
    public class GameOverBehaviour : MonoBehaviour
    {
        private GameState gameState;

        public TMP_Text deathText;
        public GameObject retryContainer;
        public TMP_Text remainingLivesText;
        public TMP_Text quitButtonText;

        public void OnRetryClicked()
        {
            gameState.Penguin.Position = default;
            gameState.DeathReason = default;
            Assets.Scripts.Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.Game);
        }

        public void OnToMainMenuButtonClicked()
        {
            Base.Core.Game.Stop();
            Assets.Scripts.Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.MainMenu);
        }

        private void OnGameInitialized()
        {
            this.gameState = Base.Core.Game.State;

            this.deathText.text = $"Poor {gameState.Penguin.Name} died by {gameState.DeathReason}";

            if (gameState.RemainingLives > 0)
            {
                retryContainer.SetActive(true);
                this.remainingLivesText.text = String.Format("You have {0} lives remaining.", gameState.RemainingLives);
                quitButtonText.text = "No, i'd rather quit";
            }
            else
            {
                retryContainer.SetActive(false);
                quitButtonText.text = "Quit :(";
            }
        }

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(OnGameInitialized);
        }
    }
}
