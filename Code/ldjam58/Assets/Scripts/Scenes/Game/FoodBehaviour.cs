using Assets.Scripts.Core.Model;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scenes.Game
{
    public class FoodBehaviour : MonoBehaviour
    {

        public Food Food { get; protected set; }

        public void Init( Food food)
        {
            this.Food = food;
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body != default)
            {
                if (collision.body.gameObject.TryGetComponent<PenguinBehaviour>(out _))
                {
                }
            }
        }
    }
}
