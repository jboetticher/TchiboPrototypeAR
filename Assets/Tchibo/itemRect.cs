using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class itemRect : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<TchiboDataManager>().addRect = GetComponent<RectTransform>();
        Debug.Log("yo");
    }
}