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

        //private void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log($"Trigger Collided with {other.gameObject.name}");
        //}

        //private void OnCollisionEnter(Collision collision)
        //{
        //    Debug.Log($"Collision with {collision.gameObject.name}");
        //}
    }
}
