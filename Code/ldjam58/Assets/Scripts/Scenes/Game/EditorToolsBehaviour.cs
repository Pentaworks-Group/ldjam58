using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class EditorToolsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera Camera;

        private EditorToolBehaviour selectedTool;

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
            RaycastHit hit;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))

            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }

        }

    }
}