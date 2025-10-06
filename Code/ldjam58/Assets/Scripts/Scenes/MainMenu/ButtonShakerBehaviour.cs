using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
	public class ButtonShakerBehaviour : MonoBehaviour
	{
		private float shakeSpeed = 1.0f;
		private Vector3 origPosition;


        private void Awake()
        {
            origPosition = transform.position;
        }

        private void Shake()
        {
            ShakeRotation();
            ShakePosition();
        }

        private void ShakeRotation()
        {
        }

        private void ShakePosition()
        {
        }


    }
}