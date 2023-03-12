using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platinio.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public string CharacterStory = "";

    //Drag
    private bool _dragging = false;
    private Vector3 _offset;

    //UI Objects
    public GameObject WorldCanvas;
    public GameObject ScreenCanvas;
    public TMP_Text Response;
    public GameObject ConversationInputField;

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
        WorldCanvas.GetComponent<CanvasGroup>().FadeIn(0.5f);
        ScreenCanvas.GetComponent<CanvasGroup>().FadeIn(0.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        WorldCanvas.GetComponent<CanvasGroup>().FadeOut(0.5f);
        ScreenCanvas.GetComponent<CanvasGroup>().FadeOut(0.5f);
    }

    public void StopMovement()
    {
        Player.Instance.DisableMovement();
    }

    public void StartMovement()
    {
        Player.Instance.EnableMovement();
    }

    public void GenerateCharacterResponse()
    {
        ConversationInputField.GetComponent<TMP_InputField>().SetTextWithoutNotify("");
        DataManager.Instance.GenerateCharacterResponse(Response);
    }
}
