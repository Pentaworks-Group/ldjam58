
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class CameraBehaviour : MonoBehaviour
    {
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
        }

        public void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Scroll");
            moveAction.performed -= HandleScroll;

        }

        private void HandleScroll(InputAction.CallbackContext context)
        {
            float z = context.ReadValue<float>();
            var pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y - z, pos.z);
        }



    }
}