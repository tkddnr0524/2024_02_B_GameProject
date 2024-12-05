using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FloatingTextManager : MonoBehaviour
{

    private static FloatingTextManager instance;
    public static FloatingTextManager Instance => instance;
    public GameObject floatingTextPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void ShowFloatingText(string text, Vector3 position)
    {
        var go = Instantiate(floatingTextPrefab, position + Vector3.up * 2f, Quaternion.identity);
        go.GetComponent<TextMeshPro>().text = text;
        Destroy(go, 2f);
    }

}
