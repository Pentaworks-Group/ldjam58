using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace Assets.Scripts
{
    public enum TextResizePattern
    {
        IgnoreRichText,
        AllCharacters,
    }

    [ExecuteAlways]
    public class TextAutoSizeController : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> _labels = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> elementsToIgnore = new List<TMP_Text>();
        [SerializeField] private TextResizePattern _pattern;
        [SerializeField] private bool _executeOnUpdate;
        [SerializeField] private bool _searchChildren = false;
        private int _currentIndex;
        private float smallestSize;

        private void Start()
        {
            GatherTexts();
            SetAutoResizeTrue();
            SetSmallestSize();
        }

        public void AddLabel(TMP_Text label)
        {
            if (label != null)
            {
                this._labels.Add(label);
            }
        }

        private void GatherTexts()
        {
            var tt = transform.GetComponentsInChildren<TMP_Text>(true);
            _labels = new List<TMP_Text>(tt);

            foreach (var element in elementsToIgnore)
            {
                _labels.Remove(element);
            }
        }

        private void SetAutoResizeTrue()
        {
            Debug.Log("Resizing");
            smallestSize = 999999999;
            foreach (var element in _labels)
            {
                element.enableAutoSizing = true;
                element.ForceMeshUpdate();
                element.enableAutoSizing = false;
                smallestSize = Mathf.Min(smallestSize, element.fontSize);
            }
        }

        private void SetSmallestSize()
        {
            foreach (var element in _labels)
            {
                element.fontSize = smallestSize;
            }
        }
    }
}
