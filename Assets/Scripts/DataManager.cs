using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Platinio.UI;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.Networking;
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    //Instance
    public static DataManager Instance;

    //Temp
    public bool Generating = false;
    public Sprite MapSprite;
    public GameObject GameScene;
    public GameObject CharacterPrefab;
    public GameObject CurCharacter = null;

    //Game Data
    public GameObject Map;
    public string BackgroundStory = "";
    public List<GameObject> CharacterList = new List<GameObject>();

    //ChatGPT
    public string ChatGPTKey;

    void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    //AI Generate Content API
    public void GenerateMap(string description)
    {
        if (Generating)
        {
            Debug.Log("Generating...Do not send multiple requests.");
            return;
        }
        //Generating = true;
        Map.SetActive(true);
        Map.GetComponent<SpriteRenderer>().sprite = MapSprite;
        Map.GetComponent<SpriteRenderer>().FadeIn(1.0f);

    }

    public void GenerateBackgroundStory(TMP_InputField inputField, string description)
    {
        if (Generating)
        {
            Debug.Log("Generating...Do not send multiple requests.");
            return;
        }
        //Generating = true;
        BackgroundStory = "TODO: Replace with AIGC";
        inputField.SetTextWithoutNotify("TODO: Replace with AIGC");
    }

    public void GenerateCharacter(string description)
    {
        if (Generating)
        {
            Debug.Log("Generating...Do not send multiple requests.");
            return;
        }
        Generating = true;
        Prompt prompt = new Prompt();
        prompt.PromptText = description;
        if (CurCharacter == null)
        {   
            CurCharacter = Instantiate(CharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            CurCharacter.transform.parent = GameScene.transform;
            CharacterList.Add(CurCharacter);
            StartCoroutine(GenerateCharacterImage(prompt));
        } else
        {
            StartCoroutine(GenerateCharacterImage(prompt));
        }
    }

    public void GenerateCharacterStory(TMP_InputField inputField, string description)
    {
        if (Generating)
        {
            Debug.Log("Generating...Do not send multiple requests.");
            return;
        }
        //Generating = true;
        CurCharacter.GetComponent<Character>().CharacterStory = "TODO: Replace with AIGC";
        inputField.SetTextWithoutNotify("TODO: Replace with AIGC");
    }

    public void GenerateCharacterResponse(TMP_Text text)
    {
        if (Generating)
        {
            Debug.Log("Generating...Do not send multiple requests.");
            return;
        }
        //Generating = true;
        text.text = "TODO: Replace with AIGC";
    }

    #region IEnumerators
    IEnumerator GenerateCharacterImage(Prompt prompt)
    {
        string url = "http://ai.dreamin.land/api/gen_character/";
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        Encoding encoding = Encoding.UTF8;

        string json = JsonConvert.SerializeObject(prompt);
        byte[] buffer = encoding.GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(buffer);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Generate Character Image Succcess!");
        }

        //read and store in gameData
        ImageData d = JsonConvert.DeserializeObject<ImageData>(webRequest.downloadHandler.text);
        StartCoroutine(GetCharacterImage(d.ImageURL));
    }

    IEnumerator GetCharacterImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error + "imageLink: " + url);
        }
        else
        {
            Debug.Log("Get Character Image Succcess!");
            Texture2D t = ((DownloadHandlerTexture)www.downloadHandler).texture;
            t.filterMode = FilterMode.Point;

            //Instantiate Character Gameobject
            CurCharacter.GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
            CurCharacter.GetComponent<SpriteRenderer>().FadeIn(1.0f);
        }

        Generating = false;
    }

    IEnumerator GenerateText(TMP_Text response, Chat chat)
    {
        string requestBody = JsonConvert.SerializeObject(chat);
        byte[] requestBodyBytes = System.Text.Encoding.UTF8.GetBytes(requestBody);

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ChatGPTKey);
        request.uploadHandler = new UploadHandlerRaw(requestBodyBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.timeout = 10;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.DataProcessingError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            // Process the responseText as needed
            Debug.Log(responseText);
        }
    }


    #endregion
}
