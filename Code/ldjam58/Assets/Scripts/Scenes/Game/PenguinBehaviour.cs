using Assets.Scripts.Core.Model;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Scenes.Game
{
    public class PenguinBehaviour : MonoBehaviour
    {
        private Penguin penguin;
        private Rigidbody penguinRigidbody;

        private Vector3 dragStart;
        private float strength = 0.1f;
        private float maxStrength = 20f;
        private bool isDragging = false;
        [SerializeField]
        private Transform arrow;

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

            Camera.main.transform.position = Vector3.Lerp(currentCameraPosition, new Vector3(this.transform.position.x, currentCameraPosition.y, this.transform.position.z), 0.1f);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                dragStart = Mouse.current.position.ReadValue();
                arrow.gameObject.SetActive(true);
                isDragging = true;
            }
            else
            {
                if (isDragging)
                {
                    Vector3 mousePos = Mouse.current.position.ReadValue();
                    var pos = transform.position;
                    var x = mousePos.x - dragStart.x;
                    var y = mousePos.y - dragStart.y;
                    if (Mathf.Abs(x) > 0.1f && Mathf.Abs(y) > 0.1f)
                    {
                        var direction = new Vector3(x, 0, y);
                        arrow.rotation = Quaternion.LookRotation(direction);
                        var appliedStrength = Mathf.Min(direction.magnitude * strength, 5);
                        arrow.localScale = new Vector3(1, 1, appliedStrength);
                    }
                }
                if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
                {

                    Vector3 mousePos = Mouse.current.position.ReadValue();

                    var x = (mousePos.x - dragStart.x);
                    var y = (mousePos.y - dragStart.y);
                    if (Mathf.Abs(x) > 0.1f && Mathf.Abs(y) > 0.1f)
                    {
                        var direction = new Vector3(x, 0, y);
                        var appliedStrength = Mathf.Min(direction.magnitude * strength, maxStrength);
                        direction = direction.normalized * appliedStrength;
                        penguinRigidbody.AddForce(direction, ForceMode.Impulse);
                        transform.rotation = Quaternion.LookRotation(direction);
                        isDragging = false;
                        arrow.gameObject.SetActive(false);
                    }
                }
            }

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
