using UnityEngine;

namespace Assets.Scripts.Scenes.GameOver
{
    public class GameOverBehaviour : MonoBehaviour
    {
        public void OnToMainMenuButtonClicked()
        {
            Base.Core.Game.Stop();
            Assets.Scripts.Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.MainMenu);
        }
    }
}
