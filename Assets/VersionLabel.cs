using i5.Editor.Versioning;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionLabel : MonoBehaviour
{
    public TextMeshPro label;

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.text = VersionManager.DataInstance.VersionString;
    }
}
