
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class EditorToolsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera Camera;
        [SerializeField]
        private LayerMask raycastLayerMask;

        private EditorToolBehaviour selectedTool;

        private EventSystem eventSystem;
        private bool isOverUI;



        void Start()
        {
            eventSystem = EventSystem.current;
        }

        void Update()
        {
            isOverUI = eventSystem.IsPointerOverGameObject();
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
            var moveAction = InputSystem.actions.FindAction("Click");
            moveAction.performed += ExecuteClick;
        }

        public void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Click");
            moveAction.performed -= ExecuteClick;
        }

        public void SaveMap()
        {

        }

        public void SelectTool(EditorToolBehaviour toolToSelect)
        {
            if (selectedTool != null)
            {
                selectedTool.DeselectButton();
            }
            selectedTool = toolToSelect;
            toolToSelect.SelectButton();
        }

        public void ExecuteClick(InputAction.CallbackContext context)
        {
            if (selectedTool != null)
            {
                selectedTool.ExecuteClick();
            }
        }

        public void RaiseLevel()
        {
            MakeRaycast();
        }

        public void LowerLevel()
        {
            MakeRaycast();
        }

        private void MakeRaycast()
        {
            if (isOverUI)
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                Vector2Int terrainHitPoint = new Vector2Int((int)hit.point.x, (int)hit.point.z);
                Debug.Log("Did Hit " + hit.collider.gameObject.name + " x: " + hit.point.x + " z: " + hit.point.z);
            }
            else
            {
                Debug.Log("Did not Hit");
            }

        }

    }
}