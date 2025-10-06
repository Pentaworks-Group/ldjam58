
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class CameraBehaviour : MonoBehaviour
    {
        private float prevMagnitude = 0;
        private int touchCount = 0;
        private float touchZoomFactor = .5f;
        private InputAction touch0contact;
        private InputAction touch1contact;
        private InputAction touch0pos;
        private InputAction touch1pos;


        private void DefineActions()
        {
            touch0contact = new InputAction
            (
                type: InputActionType.Button,
                binding: "<Touchscreen>/touch0/press"
            );
            touch0contact.Enable();
            touch1contact = new InputAction
            (
                type: InputActionType.Button,
                binding: "<Touchscreen>/touch1/press"
            );
            touch1contact.Enable();



            touch0pos = new InputAction
            (
                type: InputActionType.Value,
                binding: "<Touchscreen>/touch0/position"
            );
            touch0pos.Enable();
            touch1pos = new InputAction
            (
                type: InputActionType.Value,
                binding: "<Touchscreen>/touch1/position"
            );
            touch1pos.Enable();
        }

        private void IncreaseCountTouch(InputAction.CallbackContext contex)
        {
            touchCount++;
        }

        private void DecreaseCountTouch(InputAction.CallbackContext contex)
        {
            touchCount--;
            prevMagnitude = 0;
        }

        private void PerformTouchZoom(InputAction.CallbackContext contex)
        {
            if (touchCount < 2)
            {
                return;
            }
            var magnitude = (touch0pos.ReadValue<Vector2>() - touch1pos.ReadValue<Vector2>()).magnitude;
            if (prevMagnitude == 0)
            {
                prevMagnitude = magnitude;
            }
            var difference = magnitude - prevMagnitude;
            prevMagnitude = magnitude;
            MoveCam(difference * touchZoomFactor);
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

            DefineActions();

            touch0contact.performed += IncreaseCountTouch;
            touch1contact.performed += IncreaseCountTouch;
            touch0contact.canceled += DecreaseCountTouch;
            touch1contact.canceled += DecreaseCountTouch;
            touch1pos.performed += PerformTouchZoom;
        }

        public void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Scroll");
            moveAction.performed -= HandleScroll;


            touch0contact.performed -= IncreaseCountTouch;
            touch1contact.performed -= IncreaseCountTouch;
            touch0contact.canceled -= DecreaseCountTouch;
            touch1contact.canceled -= DecreaseCountTouch;
            touch1pos.performed -= PerformTouchZoom;
        }

        private void HandleScroll(InputAction.CallbackContext context)
        {
            float z = context.ReadValue<float>();
            MoveCam(z);
        }

        private void MoveCam(float z)
        {
            var pos = this.transform.position;
            var newHight = Mathf.Max(pos.y - z, 10);
            this.transform.position = new Vector3(pos.x, newHight, pos.z);
        }
    }
}