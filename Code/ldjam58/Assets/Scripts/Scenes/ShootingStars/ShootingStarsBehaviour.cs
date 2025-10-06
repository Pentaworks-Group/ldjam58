using System.Collections.Generic;

using Assets.Scripts.Core;

using GameFrame.Core.Extensions;
using GameFrame.Core.Math;

using TMPro;

using UnityEngine;

using UnityVector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.ShootingStars
{
    public class ShootingStarsBehaviour : MonoBehaviour
    {
        private GameState gameState;

        public GameObject floppy;

        public TMP_Text deathText;
        public GameObject retryContainer;
        public TMP_Text remainingLivesText;
        public TMP_Text quitButtonText;
        public AutoRotateBehaviour floppyRotator;

        private UnityVector3 floppyStartPosition;
        private UnityVector3 floppyTargetPosition;
        private float speedFactor;

        private SpawnBox lastSpawnBox;
        private readonly List<SpawnBox> spawnBoxes = new List<SpawnBox>();
        private readonly Range movementSpeedRange = new Range(0.75f, 3);
        private readonly Range rotationFactorRange = new Range(0.75f, 2);

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
            InitializeBoxes(Camera.main);

            lastSpawnBox = spawnBoxes.GetRandomEntry();

            var spawnPosition = lastSpawnBox.GetRandomPosition(0.3f);

            floppy.transform.position = spawnPosition;

            var targetBox = spawnBoxes.GetRandomEntry((b) => b != lastSpawnBox);

            var targetPosition = targetBox.GetRandomPosition(0.3f);

            floppyTargetPosition = targetPosition;

            speedFactor = movementSpeedRange.GetRandom();

            floppyRotator.speedFactor = rotationFactorRange.GetRandom();
            floppyRotator.direction = UnityEngine.Random.Range(0, 1) > 0.5 ? -1 : 1;

            this.gameState = Base.Core.Game.State;

            this.deathText.text = GetRandomDeathText();

            if (gameState.RemainingLives > 0)
            {
                retryContainer.SetActive(true);
                this.remainingLivesText.text = string.Format("You have {0} lives remaining.", gameState.RemainingLives);
                quitButtonText.text = "No, i'd rather quit";
            }
            else
            {
                retryContainer.SetActive(false);
                quitButtonText.text = "Quit :(";
            }
        }

        private System.String GetRandomDeathText()
        {
            var randomValue = Random.Range(0f, 1f);

            if (randomValue > 0.66666)
            {
                return string.Format("Poor {0} went to outer space...\nhe suffocated.", gameState.Penguin.Name);
            }
            else if (randomValue > 0.33333)
            {
                return string.Format("{0} was abducted by aliens...", gameState.Penguin.Name);
            }
            else if (randomValue > 0.05)
            {
                return string.Format("{0} couldn't stand you anymore.", gameState.Penguin.Name);
            }
            else
            {
                return string.Format("{0} Had enough of your shit.", gameState.Penguin.Name);
            }
        }

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(OnGameInitialized);
        }

        private void InitializeBoxes(Camera camera)
        {
            var offset = 50;
            var boxSize = 5;

            spawnBoxes.Add(new SpawnBox("left")
            {
                Rectangle = new Rect(-offset, 0, boxSize, camera.pixelHeight)
            });

            spawnBoxes.Add(new SpawnBox("top")
            {
                Rectangle = new Rect(0, camera.pixelHeight + offset, camera.pixelWidth, boxSize)
            });

            spawnBoxes.Add(new SpawnBox("right")
            {
                Rectangle = new Rect(camera.pixelWidth + offset, 0, boxSize, camera.pixelHeight)
            });

            spawnBoxes.Add(new SpawnBox("bottom")
            {
                Rectangle = new Rect(0, -offset, camera.pixelWidth, boxSize)
            });
        }

        private void Update()
        {
            if (gameState != default)
            {
                var distance = (floppyTargetPosition - floppy.transform.position).magnitude;

                if (distance < 10)
                {
                    var targetBox = spawnBoxes.GetRandomEntry((b) => b != lastSpawnBox);

                    var targetPosition = targetBox.GetRandomPosition(0.3f);

                    Debug.Log(string.Format("{0} => {1} ({2} => {3})", lastSpawnBox, targetBox, floppyTargetPosition, targetPosition));

                    lastSpawnBox = targetBox;

                    floppyTargetPosition = targetPosition;

                    speedFactor = movementSpeedRange.GetRandom();
                    floppyRotator.speedFactor = rotationFactorRange.GetRandom();
                    floppyRotator.direction = UnityEngine.Random.Range(0f, 1f) > 0.5f ? -1 : 1;
                }

                floppy.transform.position = UnityVector3.Lerp(floppy.transform.position, floppyTargetPosition, speedFactor * Time.deltaTime);
            }
        }
    }
}
