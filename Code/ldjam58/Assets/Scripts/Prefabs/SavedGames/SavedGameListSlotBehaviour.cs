using System;

using Assets.Scripts.Core.Persistence;
using Assets.Scripts.Extensions;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Prefabs.Menus
{
    public class SavedGameListSlotBehaviour : GameFrame.Core.UI.List.ListSlotBehaviour<SavedGamePreview>
    {
        private TMP_Text savedOnText;
        private TMP_Text timePlayedText;
        private TMP_Text levelText;
        private TMP_Text scoreText;

        public override void RudeAwake()
        {
            if (!transform.TryFindAndGetComponent("Content/SavedOnText", out savedOnText))
            {
                throw new System.Exception("Failed to get Text 'SavedOnText'!");
            }

            if (!transform.TryFindAndGetComponent("Content/TimePlayedText", out timePlayedText))
            {
                throw new System.Exception("Failed to get Text 'TimePlayedText'!");
            }

            if (!transform.TryFindAndGetComponent("Content/LevelText", out levelText))
            {
                throw new System.Exception("Failed to get Text 'LevelText'!");
            }

            if (!transform.TryFindAndGetComponent("Content/ScoreText", out scoreText))
            {
                throw new System.Exception("Failed to get Text 'ScoreText'!");
            }
        }

        public override void UpdateUI()
        {
            this.savedOnText.text = this.content.SavedOn.ToString("G");
            this.timePlayedText.text = TimeSpan.FromSeconds(this.content.TimeElapsed).ToString("hh\\:mm\\:ss");
            this.scoreText.text = this.content.Score.ToString("G");
            this.levelText.text = this.content.Level;
        }
    }
}
