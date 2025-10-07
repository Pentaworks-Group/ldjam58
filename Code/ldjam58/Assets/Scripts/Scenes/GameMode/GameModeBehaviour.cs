using System;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Scenes.Menues;
using Assets.Scripts.Scenes.ShootingStars;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.GameMode
{
    public class GameModeBehaviour : BaseMenuBehaviour
    {
        private readonly Dictionary<String, GameModeButtonBehaviour> gameModeButtonBehaviours = new Dictionary<string, GameModeButtonBehaviour>();

        public GameObject gameModeTemplate;
        public Transform gameModeContainer;

        private SpawnBox spawnBox;

        public void GameModeSelected(GameModeButtonBehaviour gameModeBehaviour)
        {
            Base.Core.Game.Start(gameModeBehaviour.GameMode);
        }

        private void LoadGameModes()
        {
            var gameModes = Base.Core.Game.GetAvailableGameModes();

            if (gameModes.Count > 1)
            {
                foreach (var gameMode in gameModes)
                {
                    SpawnGameModeIsland(gameMode);
                }
            }
            else if (gameModes.Count == 1)
            {
                Base.Core.Game.Start(gameModes[0]);
            }
        }

        private void SpawnGameModeIsland(Core.Definitions.GameMode gameMode)
        {
            var copy = Instantiate(gameModeTemplate, gameModeContainer);

            if (copy.TryGetComponent<GameModeButtonBehaviour>(out var gameModeButtonBehaviour))
            {
                gameModeButtonBehaviour.Init(gameMode);

                var button = copy.GetComponent<Button>();
                button.onClick.AddListener(() => GameModeSelected(gameModeButtonBehaviour));

                gameModeButtonBehaviours[gameMode.Reference] = gameModeButtonBehaviour;

                copy.transform.position = GetRandomPositionOnScreen();

                copy.SetActive(true);
            }
        }

        private Vector3 GetRandomPositionOnScreen()
        {
            for (int i = 0; i < 10; i++)
            {
                var position = spawnBox.GetRandomPosition(0);

                if (true)
                {
                    return position;
                }
            }

            return default;
        }

        private void Awake()
        {
            this.spawnBox = new SpawnBox("Camera")
            {
                Rectangle = new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight)
            };

            LoadGameModes();
        }
    }
}
