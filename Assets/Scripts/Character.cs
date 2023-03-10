using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platinio.UI;

public class Character : MonoBehaviour
{
    private bool _dragging = false;
    private Vector3 _offset;
    public string CharacterStory = "";
    public GameObject UICanvas;

    private void Update()
    {
        if (_dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        }
    }

    private void OnMouseDown()
    {   
        if (EditorManager.Instance.GetPhase() == EditorManager.Phase.Finished)
        {
            return;
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager.Instance.ConversationInputField.SetActive(true);
        DataManager.Instance.ConversationInputField.GetComponent<CanvasGroup>().FadeIn(1);
        UICanvas.GetComponent<CanvasGroup>().FadeIn(1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DataManager.Instance.ConversationInputField.GetComponent<CanvasGroup>().FadeOut(1).SetOnComplete(delegate { DataManager.Instance.ConversationInputField.SetActive(true); });
        UICanvas.GetComponent<CanvasGroup>().FadeOut(1);
    }
}
