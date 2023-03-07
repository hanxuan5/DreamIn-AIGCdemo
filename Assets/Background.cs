using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject InputField;
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(450f, 40.24f + InputField.GetComponent<RectTransform>().sizeDelta.y);
    }
}
