using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platinio.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class EditorManager : MonoBehaviour
{
    public enum Phase
    {
        Initial,
        CreatingMap,
        CreatingBackgroundStory,
        CreatingCharacter,
        CreatingCharacterStory,
        Finished
    }

    //Phase
    private Phase _phase = Phase.Initial;

    //UI Objects
    public GameObject Background;
    public GameObject Diamond;
    public GameObject Line;
    public GameObject NextButton;
    public GameObject AIButton;
    public GameObject InputField;
    public TMP_Text Description;
    public GameObject Prompt;
    public GameObject StartGameButton;

    //Camera Movement & Camera Zoom
    private Vector3 _origin;
    private Vector3 _offset;
    private bool _onDrag;
    private float _zoomChange = 500f;

    //Instance
    public static EditorManager Instance;

    public GameObject Player;

    void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public Phase GetPhase()
    {
        return _phase;
    }

    #region Camera Movement
    private void LateUpdate()
    {
        if (_phase == Phase.Initial || _phase == Phase.Finished)
        {
            return;
        }

        //Change the position of Camera
        if (Input.GetMouseButton(1))
        {
            _offset = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (_onDrag == false)
            {
                _onDrag = true;
                _origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            //Change the size of Camera
            if (Input.mouseScrollDelta.y > 0)
            {
                Camera.main.orthographicSize -= _zoomChange * Time.deltaTime;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                Camera.main.orthographicSize += _zoomChange * Time.deltaTime;
            }
        } else
        {
            _onDrag = false;
        }

        if (_onDrag)
        {
            Camera.main.transform.position = _origin - _offset;
        }
    }
    #endregion

    #region NextPhaseButton
    public void NextPhaseButton()
    {
        switch(_phase)
        {
            case Phase.Initial:
                StartCreatingMap();
                break;
            case Phase.CreatingMap:
                StartCreatingBackgroundStory();
                break;
            case Phase.CreatingBackgroundStory:
                StartCreatingCharacterV0();
                break;
            case Phase.CreatingCharacter:
                StartCreatingCharacterStory();
                break;
            case Phase.CreatingCharacterStory:
                StartCreatingCharacterV1();
                break;
        }
    }

    private void StartCreatingMap()
    {
        //Animation
        Diamond.transform.ScaleTween(Vector3.one * 0.3f, 1.0f);
        Diamond.GetComponent<RectTransform>().Move(new Vector2(-180.0f, 100.0f), 1.0f);
        Line.transform.ScaleTween(new Vector3(4.0f, 0.01f, 1.0f), 1.0f).SetDelay(1.0f);
        NextButton.SetActive(true);
        NextButton.GetComponent<CanvasGroup>().FadeIn(1.0f).SetDelay(1.0f);
        AIButton.SetActive(true);
        AIButton.GetComponent<CanvasGroup>().FadeIn(1.0f).SetDelay(1.0f);
        Prompt.GetComponent<TMP_Text>().SetText("Where Are You?");
        Prompt.GetComponent<CanvasGroup>().FadeIn(1.0f).SetDelay(1.0f);
        InputField.SetActive(true);
        InputField.GetComponent<CanvasGroup>().FadeIn(1.0f).SetDelay(1.0f);
        Background.GetComponent<CanvasGroup>().FadeIn(1.0f).SetDelay(1.0f);

        //Set Phase
        _phase = Phase.CreatingMap;
    }

    private void StartCreatingBackgroundStory()
    {  
        if (DataManager.Instance.Map.GetComponent<SpriteRenderer>().color != Color.white)
        {
            Debug.Log("Create A Map First!");
            return;
        }

        Prompt.GetComponent<TMP_Text>().SetText("What's happening here?");
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify("");
        //Set Phase
        _phase = Phase.CreatingBackgroundStory;
    }

    private void StartCreatingCharacterV0()
    {
        if (DataManager.Instance.BackgroundStory == "")
        {
            Debug.Log("Create A Background Story First!");
            return;
        }

        Prompt.GetComponent<TMP_Text>().SetText("Anyone is here?");
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify("");

        //Set Phase
        _phase = Phase.CreatingCharacter;
    }

    private void StartCreatingCharacterStory()
    {
        if (DataManager.Instance.CurCharacter == null)
        {
            Debug.Log("Create A Character First!");
            return;
        }

        Prompt.GetComponent<TMP_Text>().SetText("What's happening to this person?");
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify("");

        //Set Phase
        _phase = Phase.CreatingCharacterStory;
    }

    private void StartCreatingCharacterV1()
    {
        if (DataManager.Instance.CurCharacter.GetComponent<Character>().CharacterStory == "")
        {
            Debug.Log("Create A Character Story First!");
            return;
        }

        DataManager.Instance.CurCharacter = null;
        Prompt.GetComponent<TMP_Text>().SetText("Anyone is here?");
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify("");

        //Set Phase
        _phase = Phase.CreatingCharacter;

        //Show Start Game Button
        StartGameButton.SetActive(true);
        StartGameButton.GetComponent<CanvasGroup>().FadeIn(1.0f);
    }

    public void StartGame()
    {
        if (DataManager.Instance.CurCharacter != null)
        {
            Debug.Log("Finish Current Character First!");
            return;
        }

        Background.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { Background.SetActive(false); });
        Diamond.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { Diamond.SetActive(false); });
        Line.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { Line.SetActive(false); });
        NextButton.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { NextButton.SetActive(false); });
        AIButton.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { AIButton.SetActive(false); });
        InputField.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { InputField.SetActive(false); });
        Prompt.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { Prompt.SetActive(false); });
        StartGameButton.GetComponent<CanvasGroup>().FadeOut(1.0f).SetOnComplete(delegate { StartGameButton.SetActive(false); });
        
        Player.SetActive(true);
        Player.GetComponent<SpriteRenderer>().FadeIn(1.0f);
    }


    #endregion

    #region AIGenerateButton
    public void AIGenerateButton()
    {
        if (Description.text.Length < 5)
        {
            Debug.Log("Input Description First!");
            return;
        }

        switch (_phase)
        {
            case Phase.CreatingMap:
                GenerateMap();
                break;
            case Phase.CreatingBackgroundStory:
                GenerateBackgroundStory();
                break;
            case Phase.CreatingCharacter:
                GenerateCharacter();
                break;
            case Phase.CreatingCharacterStory:
                GenerateCharacterStory();
                break;

        }
    }

    private void GenerateMap()
    {
        DataManager.Instance.GenerateMap(Description.text);
    }

    private void GenerateBackgroundStory()
    {
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify(DataManager.Instance.GenerateBackgroundStory(Description.text));
    }

    private void GenerateCharacter()
    {
        DataManager.Instance.GenerateCharacter(Description.text);
    }

    private void GenerateCharacterStory()
    {
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify(DataManager.Instance.GenerateCharacterStory(Description.text));
    }

    #endregion
}
