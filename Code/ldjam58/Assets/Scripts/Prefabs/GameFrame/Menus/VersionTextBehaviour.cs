using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionTextBehaviour : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TMPro.TMP_Text>().text = $"{Application.version}";
    }
}
