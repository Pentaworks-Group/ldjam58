
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class CameraBehaviour : MonoBehaviour
    {
        private float prevMagnitude = 0;
        private int touchCount = 0;

        private void Start()
        {
            // pinch gesture
            var touch0contact = new InputAction
            (
                type: InputActionType.Button,
                binding: "<Touchscreen>/touch0/press"
            );
            touch0contact.Enable();
            var touch1contact = new InputAction
            (
                type: InputActionType.Button,
                binding: "<Touchscreen>/touch1/press"
            );
            touch1contact.Enable();

            touch0contact.performed += _ => touchCount++;
            touch1contact.performed += _ => touchCount++;
            touch0contact.canceled += _ =>
            {
                touchCount--;
                prevMagnitude = 0;
            };
            touch1contact.canceled += _ =>
            {
                touchCount--;
                prevMagnitude = 0;
            };

            var touch0pos = new InputAction
            (
                type: InputActionType.Value,
                binding: "<Touchscreen>/touch0/position"
            );
            touch0pos.Enable();
            var touch1pos = new InputAction
            (
                type: InputActionType.Value,
                binding: "<Touchscreen>/touch1/position"
            );
            touch1pos.Enable();
            touch1pos.performed += _ =>
            {
                if (touchCount < 2)
                    return;
                var magnitude = (touch0pos.ReadValue<Vector2>() - touch1pos.ReadValue<Vector2>()).magnitude;
                if (prevMagnitude == 0)
                    prevMagnitude = magnitude;
                var difference = magnitude - prevMagnitude;
                prevMagnitude = magnitude;
                MoveCam(-difference);
            };
        }
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