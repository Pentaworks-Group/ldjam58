using System;

using Assets.Scripts.Core.Persistence;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Prefabs.Menus
{
    public class SavedGameMenuBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SavedGameListContainerBehaviour listContainer;
        [SerializeField]
        private GameObject detailsContainer;
        [SerializeField]
        private TMP_Text savedOnText;
        [SerializeField]
        private TMP_Text startedOnText;
        [SerializeField]
        private TMP_Text scoreText;
        [SerializeField]
        private TMP_Text levelText;
        [SerializeField]
        private TMP_Text timeElapsedText;

        private SavedGamePreview currentSlot;

        public void Awake()
        {
            if (this.detailsContainer != null)
            {
                this.detailsContainer.SetActive(false);
            }
        }

        public void SaveGame()
        {
            Base.Core.Game.SaveGame();

            listContainer.UpdateList();
        }

        public void DeleteSelectedSavedGame()
        {
            if (currentSlot != null)
            {
                Base.Core.Game.DeleteSavedGame(currentSlot.Key);
                listContainer.UpdateList();

                Base.Core.Game.PlayButtonSound();
            }
        }

        public void LoadSelectedSavedGame()
        {
            if (currentSlot != null)
            {
                Base.Core.Game.LoadSavedGame(currentSlot.Key);
                listContainer.UpdateList();

                Base.Core.Game.PlayButtonSound();
            }
        }

        public void OnSlotSelected(SavedGamePreview selectedSlot)
        {
            currentSlot = selectedSlot;

            UpdateDetails(selectedSlot);
        }

        private void UpdateDetails(SavedGamePreview selectedSlot)
        {
            if (currentSlot != default)
            {
                this.savedOnText.text = selectedSlot.SavedOn.ToString("G");
                this.startedOnText.text = selectedSlot.StartedOn.ToString("G");
                this.timeElapsedText.text = TimeSpan.FromSeconds(selectedSlot.TimeElapsed).ToString("hh\\:mm\\:ss");

                this.scoreText.text = selectedSlot.Score.ToString("G");
                this.levelText.text = selectedSlot.Level;

                this.detailsContainer.SetActive(true);
            }
            else
            {
                this.detailsContainer.SetActive(false);
            }
        }
    }
}
