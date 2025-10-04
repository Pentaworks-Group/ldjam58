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
                    Base.Core.Game.State.DeathReason = "Eaten by an orca";
                    Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.GameOver);
                }
            }
        }
    }
}
