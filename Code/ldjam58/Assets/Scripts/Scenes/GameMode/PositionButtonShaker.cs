using UnityEngine;

namespace Assets.Scripts.Scenes.GameMode
{
    public class PositionButtonShaker : MonoBehaviour
    {
        private float shakeSpeedX = 1.0f;
        private float shakeSpeedY = 1.3f;
        private float shakeStrengthX =10f;
        private float shakeStrengthY = 10f;
        private float delay;
        private RectTransform rectTransform;

        private Vector3 originalPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            shakeSpeedX *= Random.Range(0.8f, 1.2f);
            shakeSpeedY *= Random.Range(0.8f, 1.2f);
            shakeStrengthX *= Random.Range(0.8f, 1.2f);
            shakeStrengthY *= Random.Range(0.8f, 1.2f);
            delay = Random.Range(0, Mathf.PI * 2);
        }

        public void SetStartPosition(Vector3 startPosition)
        {
            originalPosition = startPosition;
        }

        private void Update()
        {
            if (originalPosition != default)
            {
                Shake();
            }
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
            //var x = (Mathf.Sin(Time.time) * shakeStrength) + origPosition.x;
            var x = (Mathf.Sin(Time.time * shakeSpeedX + delay) * shakeStrengthX);
            var y = (Mathf.Sin(Time.time * shakeSpeedY + delay) * shakeStrengthY);

            rectTransform.position = originalPosition + new Vector3 (x, y, 0) ;
        }
    }
}