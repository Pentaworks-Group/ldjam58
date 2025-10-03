
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        protected override GameState InitializeGameState()
        {
            return new GameState()
            { 
            };
        }

        protected override PlayerOptions InitializePlayerOptions()
        {
            var showTouchPads = false;

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                showTouchPads = true;
            }

            return new PlayerOptions()
            {
                EffectsVolume = 0.9f,
                AmbienceVolume = 0.1f,
                BackgroundVolume = 0.3f,
                //ShowTouchPads = showTouchPads
            };
        }

        protected override void RegisterScenes()
        {
            RegisterScenes(Constants.Scenes.GetAll());
        }
    }
}
