using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Scenes.ShootingStars
{
    public class AutoRotateBehaviour : MonoBehaviour
    {
        public float speedFactor = 1f;
        public float direction = 1f;

        private void Update()
        {
            this.transform.Rotate(0, 0, 360 * speedFactor * Time.deltaTime * direction);
        }
    }
}
