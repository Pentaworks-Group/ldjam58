using Assets.Scripts.Core.Model;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class PenguinBehaviour : MonoBehaviour
    {
        public UnityEvent<FoodBehaviour> Eaten = new UnityEvent<FoodBehaviour>();

        private Penguin penguin;
        private Rigidbody penguinRigidbody;

        private Vector3 dragStart;
        private Vector3 dragStartWorld;
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
                dragStartWorld = Camera.main.ScreenToWorldPoint(dragStart);
                isDragging = true;
                Debug.Log("DragStart");
            }
            else
            {
                if (isDragging)
                {
                    Vector3 mousePos = Mouse.current.position.ReadValue();
                    //Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    var pos = transform.position;
                    var x = mousePos.x - dragStart.x;
                    var y = mousePos.y - dragStart.y;
                    if (Mathf.Abs(x) > 0.1f && Mathf.Abs(y) > 0.1f)
                    {
                        // Rotate only on the Y axis (for 2D, use Vector3.forward)
                        var direction = new Vector3(x, 0, y);
                        arrow.rotation = Quaternion.LookRotation(direction);
                        var appliedStrength = Mathf.Min(direction.magnitude * strength, 5);
                        arrow.localScale = new Vector3(1, 1, appliedStrength);
                        Debug.Log("direction: " + direction);
                    }
                    else
                    {
                        Debug.Log("ZeroDist: " + dragStart + " " + dragStartWorld + " " + mousePos);
                    }
                }
                if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    Vector3 mousePos = Mouse.current.position.ReadValue();

                    var x = (mousePos.x - dragStart.x);
                    var y = (mousePos.y - dragStart.y);

                    var direction = new Vector3(x, 0, y);
                    var appliedStrength = Mathf.Min(direction.magnitude * strength, maxStrength);
                    direction = direction.normalized * appliedStrength;
                    penguinRigidbody.AddForce(direction, ForceMode.Impulse);
                    isDragging = false;
                    Debug.Log("DragStop");
                    arrow.gameObject.SetActive(false);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.parent.TryGetComponent<FoodBehaviour>(out var foodBehaviour))
            {
                Eaten.Invoke(foodBehaviour);
            }
        }
    }
}
