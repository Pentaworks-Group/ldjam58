using Assets.Scripts.Core.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class PenguinBehaviour : MonoBehaviour
    {
        private Penguin penguin;

        public void Init(Penguin penguin)
        {
            this.penguin = penguin;
        }
    }
}
