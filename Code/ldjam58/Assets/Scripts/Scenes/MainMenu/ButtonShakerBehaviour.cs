using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class ButtonShakerBehaviour : MonoBehaviour
    {
        private float shakeSpeedX = 1.0f;
        private float shakeSpeedY = 1.3f;
        private float shakeStrengthX = 0.01f;
        private float shakeStrengthY = 0.005f;
        private float delay;
        private Vector2 origAnchorMin;
        private Vector2 origAnchorMax;
        private RectTransform rectTransform;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            origAnchorMin = rectTransform.anchorMin;
            origAnchorMax = rectTransform.anchorMax;
            shakeSpeedX *= Random.Range(0.8f, 1.2f);
            shakeSpeedY *= Random.Range(0.8f, 1.2f);
            shakeStrengthX *= Random.Range(0.8f, 1.2f);
            shakeStrengthY *= Random.Range(0.8f, 1.2f);
            delay = Random.Range(0, Mathf.PI * 2);
        }

        private void Update()
        {
            Shake();
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
            rectTransform.anchorMin = new Vector2(origAnchorMin.x + x, origAnchorMin.y + y);
            rectTransform.anchorMax = new Vector2(origAnchorMax.x + x, origAnchorMax.y + y);
        }


    }
}