using Assets.Scripts.Scenes.MainMenu;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.GameMode
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(ButtonShakerBehaviour))]
    public class GameModeButtonBehaviour : MonoBehaviour
    {
        public Core.Definitions.GameMode GameMode { get; private set; }

        public void Init(Core.Definitions.GameMode gameMode)
        {
            this.GameMode = gameMode;
        }
    }
}
