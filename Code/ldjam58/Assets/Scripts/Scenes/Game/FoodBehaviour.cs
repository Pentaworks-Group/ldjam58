using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class FoodBehaviour : MonoBehaviour
    {
        private Food food;

        public void Init(Food food)
        {
            this.food = food;
        }
    }
}
