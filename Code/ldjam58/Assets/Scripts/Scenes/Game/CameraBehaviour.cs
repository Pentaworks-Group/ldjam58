
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class CameraBehaviour : MonoBehaviour
    {
        private float prevMagnitude = 0;
        private void OnEnable()
        {
            HookActions();
        }

        private void OnDisable()
        {
            UnhookActions();
        }

        public void HookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Scroll");
            moveAction.performed += HandleScroll;

            var touchPress = InputSystem.actions.FindAction("TouchMultiPress");
            moveAction.performed += HandleMultiTouch;
        }

        public void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Scroll");
            moveAction.performed -= HandleScroll;

            var touchPress = InputSystem.actions.FindAction("TouchMultiPress");
            moveAction.performed -= HandleMultiTouch;

        }

        private void HandleMultiTouch(InputAction.CallbackContext context)
        {
            Debug.Log("MultitouchContext" + context);
            //var magnitude = (touch0pos.ReadValue<Vector2>() - touch1pos.ReadValue<Vector2>()).magnitude;
            //if (prevMagnitude == 0)
            //    prevMagnitude = magnitude;
            //var difference = magnitude - prevMagnitude;
            //prevMagnitude = magnitude;
        }

        private void HandleScroll(InputAction.CallbackContext context)
        {
            float z = context.ReadValue<float>();
            MoveCam(z);
        }

        private void MoveCam(float z)
        {
            var pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y - z, pos.z);
        }
    }
}