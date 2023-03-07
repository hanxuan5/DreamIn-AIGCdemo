using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private bool _dragging = false;
    private Vector3 _offset;
    public string CharacterStory = "";

    private void Update()
    {
        if (_dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        }
    }

    private void OnMouseDown()
    {   
        if (Input.GetMouseButton(0))
        {
            _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dragging = true;
        }
    }

    private void OnMouseUp()
    {
        _dragging = false;
    }
}
