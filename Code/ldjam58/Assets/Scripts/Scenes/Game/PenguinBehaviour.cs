using Assets.Scripts.Core.Model;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class PenguinBehaviour : MonoBehaviour
    {
        private Penguin penguin;
        private Rigidbody penguinRigidbody;

        public void Init(Penguin penguin)
        {
            this.penguin = penguin;
            this.penguinRigidbody = GetComponent<Rigidbody>();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var moveVector = context.ReadValue<Vector2>();

            var translation = 5f * new Vector3(moveVector.x, 0, moveVector.y);

            penguinRigidbody.AddForce(translation, ForceMode.Impulse);
            Debug.Log($"Adding force '{translation}'");
        }

        private void HookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Move");

            moveAction.performed += OnMove;
        }

        private void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Move");

            moveAction.performed -= OnMove;
        }

        private void Update()
        {
            var currentCameraPosition = Camera.main.transform.position;

            Camera.main.transform.position = Vector3.Lerp(currentCameraPosition, new Vector3(this.transform.position.x, currentCameraPosition.y, this.transform.position.z), 0.1f );
        }

        private void OnEnable()
        {
            HookActions();
        }

        private void OnDisable()
        {
            UnhookActions();
        }
    }
}
