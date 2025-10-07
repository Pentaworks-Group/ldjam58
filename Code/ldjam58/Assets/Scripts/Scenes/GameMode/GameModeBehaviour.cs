using System;
using System.Collections.Generic;

using Assets.Scripts.Scenes.MainMenu;
using Assets.Scripts.Scenes.Menues;
using Assets.Scripts.Scenes.ShootingStars;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

namespace Assets.Scripts.Scenes.GameMode
{
    public class GameModeBehaviour : BaseMenuBehaviour
    {
        private readonly Dictionary<String, GameModeButtonBehaviour> gameModeButtonBehaviours = new Dictionary<string, GameModeButtonBehaviour>();
        private readonly GameFrame.Core.Math.Range scaleRange = new GameFrame.Core.Math.Range(0.9f, 3f);
        private readonly List<String> availableSprites = new List<String>()
        {
            "Ice_1",
            "Ice_2",
            "Ice_3",
            "Ice_4",
            "Ice_5",
            "Ice_6"
        };

        public GameObject gameModeTemplate;
        public Transform gameModeContainer;
        public TMP_Text selectedGameModeNameText;
        public TMP_Text selectedGameModeDescriptionText;
        public Button startButton;

        private SpawnBox spawnBox;
        private GameModeButtonBehaviour selectedGameModeBehaviour;

        public void GameModeSelected(GameModeButtonBehaviour gameModeBehaviour)
        {
            UpdateSelection(gameModeBehaviour);
        }

        public void Play()
        {
            Base.Core.Game.Start(selectedGameModeBehaviour.GameMode);
        }

        private void UpdateSelection(GameModeButtonBehaviour gameModeBehaviour)
        {
            this.selectedGameModeBehaviour = gameModeBehaviour;

            if (gameModeBehaviour != null)
            {
                this.selectedGameModeNameText.text = gameModeBehaviour.GameMode.Name;
                this.selectedGameModeDescriptionText.text = gameModeBehaviour.GameMode.Description;

                if (gameModeBehaviour.TryGetComponent<Button>(out var button))
                {
                    button.Select();
                }

                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }

        private void LoadGameModes()
        {
            var gameModes = Base.Core.Game.GetAvailableGameModes();

            if (gameModes.Count > 1)
            {
                foreach (var gameMode in gameModes)
                {
                    var gameModeBehaviour = SpawnGameModeIsland(gameMode);

                    if (selectedGameModeBehaviour == default && gameMode.IsDefault == true)
                    {
                        UpdateSelection(gameModeBehaviour);                        
                    }
                }

                if (selectedGameModeBehaviour == default)
                {
                    if (gameModeButtonBehaviours.TryGetValue(gameModes[0].Reference, out var button))
                    {
                        UpdateSelection(button);
                    }
                }
            }
            else if (gameModes.Count == 1)
            {
                Base.Core.Game.Start(gameModes[0]);
            }
        }

        private GameModeButtonBehaviour SpawnGameModeIsland(Core.Definitions.GameMode gameMode)
        {
            var copy = Instantiate(gameModeTemplate, gameModeContainer);

            if (copy.TryGetComponent<GameModeButtonBehaviour>(out var gameModeButtonBehaviour))
            {
                gameModeButtonBehaviour.Init(gameMode);

                var button = copy.GetComponent<Button>();
                button.onClick.AddListener(() => GameModeSelected(gameModeButtonBehaviour));

                var randomSprite = availableSprites.GetRandomEntry();

                var sprite = GameFrame.Base.Resources.Manager.Sprites.Get(randomSprite);

                gameModeButtonBehaviour.backgroundImage.sprite = sprite;

                gameModeButtonBehaviours[gameMode.Reference] = gameModeButtonBehaviour;

                copy.transform.position = spawnBox.GetRandomPosition(0.3f);

                var shake = copy.GetComponent<PositionButtonShaker>();

                shake.SetStartPosition(copy.transform.position);

                copy.SetActive(true);

                var randomScale = scaleRange.GetRandom();

                copy.transform.localScale = new UnityVector3(randomScale, randomScale, randomScale);

                return gameModeButtonBehaviour;
            }

            return default;
        }

        private void Awake()
        {
            var xMin = Camera.main.pixelWidth * 0.01f;
            var yMin = Camera.main.pixelHeight * 0.01f;
            var width = Camera.main.pixelWidth * 0.74f;
            var height = Camera.main.pixelHeight * 0.98f;

            this.spawnBox = new SpawnBox("Camera")
            {
                Rectangle = new Rect(xMin, yMin ,width ,height )
            };

            LoadGameModes();
        }
    }
}
