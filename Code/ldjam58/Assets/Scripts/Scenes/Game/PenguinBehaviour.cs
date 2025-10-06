using Assets.Scripts.Constants;
using Assets.Scripts.Core.Model;
using GameFrame.Core.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityVector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Game
{
    public class PenguinBehaviour : MonoBehaviour
    {
        public UnityEvent<FoodBehaviour> Eaten = new UnityEvent<FoodBehaviour>();

        private Penguin penguin;
        private Rigidbody penguinRigidbody;
        private Animator penguinAnimator;

        private UnityVector3 dragStart;
        private bool isDragging = false;
        [SerializeField]
        private Transform arrow;

        private Boolean IsAllowingControlWhileMoving;
        private Boolean isMoving;
        private float arrowScaler;
        private float arrowMaxLength = 10f;

        private Boolean isTouching = false;

        public void Init(Penguin penguin)
        {
            this.penguin = penguin;
            this.penguinRigidbody = GetComponent<Rigidbody>();
            this.penguinAnimator = transform.Find("Animator").GetComponent<Animator>();
            IsAllowingControlWhileMoving = Base.Core.Game.State.Mode.IsAllowingControlWhileMoving;
            arrowScaler = arrowMaxLength / penguin.MaxStrength;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var moveVector = context.ReadValue<Vector2>();

            var translation = 5f * new UnityVector3(moveVector.x, 0, moveVector.y);

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

            Camera.main.transform.position = UnityVector3.Lerp(currentCameraPosition, new UnityVector3(this.transform.position.x, currentCameraPosition.y, this.transform.position.z), 0.1f);

            if (IsAllowingControlWhileMoving || (!IsAllowingControlWhileMoving && !isMoving))
            {
                HandleMouseControl();
            }
            PositionAndVelocityWatcher();
        }

        private void PositionAndVelocityWatcher()
        {
            penguin.Position = transform.position.ToFrame();
            penguin.Velocity = penguinRigidbody.linearVelocity.ToFrame();
            if (penguin.Velocity.LengthSquared > 0.01f)
            {
                if (!isMoving)
                {
                    penguinAnimator.SetTrigger("StartSlide");
                }

                isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    penguinAnimator.SetTrigger("StartWalk");
                }

                isMoving = false;
            }
        }

        private void HandleMouseControl()
        {
            bool dragStart = false;
            bool dragStop = false;
            Vector2 pos = Vector2.zero;
            if (Touchscreen.current != null)
            {

                if (Input.touchCount >= 1)
                {
                    Touch touch = Input.GetTouch(0);
                    pos = touch.position;
                    Debug.Log("Input.touchCount >= 1: " + pos);
                    if (Input.touchCount == 1 && touch.phase == UnityEngine.TouchPhase.Began)
                    {
                        dragStart = true;
                        isTouching = true;
                    }
                    if (isTouching && touch.phase == UnityEngine.TouchPhase.Ended)
                    {
                        dragStop = true;
                        isTouching = false;
                        Debug.Log("TouchPhase.Ended");
                    }
                }
                else
                {
                    Debug.Log("Input.touchCount < 1: " + Input.touchCount);
                }

            }
            if (Mouse.current != null)
            {
                pos = Mouse.current.position.ReadValue();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    dragStart = true;
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    dragStop = true;
                }
            }
            if (dragStart)
            {
                StartDrag(pos);
            }
            else
            {
                DragHandling(pos, dragStop);
            }
        }

        private void DragHandling(Vector2 pointerPosition, bool dragStop)
        {
            if (isDragging)
            {
                var pos = transform.position;
                var x = pointerPosition.x - dragStart.x;
                var y = pointerPosition.y - dragStart.y;
                if (Mathf.Abs(x) > 0.1f && Mathf.Abs(y) > 0.1f)
                {
                    var direction = new UnityVector3(x, 0, y);
                    arrow.rotation = Quaternion.LookRotation(direction);

                    var appliedStrength = Mathf.Min(direction.magnitude * penguin.Strength, penguin.MaxStrength);
                    var arrowLegth = appliedStrength * arrowScaler;
                    arrow.localScale = new UnityVector3(1, 1, arrowLegth);

                    if (dragStop)
                    {

                        direction = direction.normalized * appliedStrength;
                        penguinRigidbody.AddForce(direction, ForceMode.Impulse);
                        transform.rotation = Quaternion.LookRotation(direction);

                        isDragging = false;
                        arrow.gameObject.SetActive(false);
                    }
                }
                else if (dragStop)
                {
                    isDragging = false;
                    arrow.gameObject.SetActive(false);
                }
            }
        }

        private void StartDrag(Vector2 position)
        {
            dragStart = position;
            arrow.gameObject.SetActive(true);
            isDragging = true;
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
            // Maybe another detection is required when different Model is used.
            if (other.gameObject.tag == Tags.Food)
            {
                var foodBehaviour = other.gameObject.GetComponentInParent<FoodBehaviour>(false);

                if (foodBehaviour != default)
                {
                    Eaten.Invoke(foodBehaviour);
                }
            }
        }
    }
}
