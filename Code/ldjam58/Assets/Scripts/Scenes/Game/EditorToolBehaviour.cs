
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scenes.Game
{
    public class EditorToolBehaviour : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent clickAction;
        private GameObject selectedImage;

        private void Awake()
        {
            selectedImage = transform.GetChild(0).gameObject;
        }

        public void ToggleButton()
        {
            selectedImage.SetActive(!selectedImage.activeSelf);
        }
        public void SelectButton()
        {
            selectedImage.SetActive(true);
        }

        public void DeselectButton()
        {
            selectedImage?.SetActive(false);
        }

        public void ExecuteClick()
        {
            clickAction.Invoke();
        }
    }
}