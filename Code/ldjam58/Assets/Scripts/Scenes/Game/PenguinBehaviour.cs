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
        private float maxDragDistance = 250f;
        private float maxArrowLength = 10;

        public void Init(Penguin penguin)
        {
            this.penguin = penguin;
            this.penguinRigidbody = GetComponent<Rigidbody>();
            this.penguinAnimator = transform.Find("Animator").GetComponent<Animator>();
            IsAllowingControlWhileMoving = Base.Core.Game.State.Mode.IsAllowingControlWhileMoving;
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

            var clickAction = InputSystem.actions.FindAction("Click");
            clickAction.started += StartDrag;
            clickAction.canceled += StopDrag;
        }

        private void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Move");
            moveAction.performed -= OnMove;

            var clickAction = InputSystem.actions.FindAction("Click");
            clickAction.started -= StartDrag;
            clickAction.canceled -= StopDrag;
        }


        private void Update()
        {
            var currentCameraPosition = Camera.main.transform.position;

            Camera.main.transform.position = UnityVector3.Lerp(currentCameraPosition, new UnityVector3(this.transform.position.x, currentCameraPosition.y, this.transform.position.z), 0.1f);


            if (isDragging)
            {
                DragHandling();
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


        private void DragHandling()
        {
            if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1)
            {
                DiscontinourDragging();
                return;
            }
            var pointerPosition = Pointer.current.position.ReadValue();
            var pos = transform.position;
            var x = pointerPosition.x - dragStart.x;
            var y = pointerPosition.y - dragStart.y;
            if (Mathf.Abs(x) > 0.3f || Mathf.Abs(y) > 0.3f)
            {
                var direction = new UnityVector3(x, 0, y);
                arrow.rotation = Quaternion.LookRotation(direction);
                var arrowLegth = GetPercentage(direction) * maxArrowLength;
                arrow.localScale = new UnityVector3(1, 1, arrowLegth);
                arrow.gameObject.SetActive(true);
            }
            else
            {
                arrow.gameObject.SetActive(false);
            }
        }

        private void StartDrag(InputAction.CallbackContext context)
        {
            if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1) {
                DiscontinourDragging();
                return; 
            }
            if (IsAllowingControlWhileMoving || (!IsAllowingControlWhileMoving && !isMoving))
            {
                var position = Pointer.current.position.ReadValue();
                dragStart = position;
                isDragging = true;
            }
        }

        private float GetPercentage(UnityVector3 direction) {
            var percentage = direction.magnitude / maxDragDistance;
            percentage = Mathf.Min(percentage, 1);
            return percentage;
        }


        public void StopDrag(InputAction.CallbackContext context)
        {
            if (isDragging)
            {
                var pointerPosition = Pointer.current.position.ReadValue();
                var pos = transform.position;
                var x = pointerPosition.x - dragStart.x;
                var y = pointerPosition.y - dragStart.y;
                if (Mathf.Abs(x) > 0.3f || Mathf.Abs(y) > 0.3f)
                {
                    var direction = new UnityVector3(x, 0, y);

                    var appliedStrength = GetPercentage(direction) * penguin.MaxStrength;
                    direction = appliedStrength * direction.normalized ;
                    penguinRigidbody.AddForce(direction, ForceMode.Impulse);
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            DiscontinourDragging();
        }

        private void DiscontinourDragging()
        {
            isDragging = false;
            arrow.gameObject.SetActive(false);
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

