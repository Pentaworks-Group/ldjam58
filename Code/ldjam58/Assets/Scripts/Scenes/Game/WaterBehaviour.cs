using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class WaterBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body != default)
            {
                if (collision.body.gameObject.TryGetComponent<PenguinBehaviour>(out _))
                {
                    var gameState = Base.Core.Game.State;

                    gameState.DeathReason = "falling into water and being eaten by an Orca.";
                    gameState.RemainingLives--;

                    Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.GameOver);
                }
            }
        }
    }
}
