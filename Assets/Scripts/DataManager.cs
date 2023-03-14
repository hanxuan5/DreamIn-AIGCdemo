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

    //Fake Data
    public Sprite MapSprite;
    public Sprite CharacterSprite;

    //Block Multiple Requests
    public bool Generating = false;

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
    public void GenerateMap(GameObject map, string description)
    {
        map.SetActive(true);
        map.GetComponent<SpriteRenderer>().sprite = MapSprite;
        map.GetComponent<SpriteRenderer>().FadeIn(1.0f);
        Generating = false;
    }

    public void GenerateBackgroundStory(TMP_InputField inputField)
    {
        Message message = new Message();
        message.Role = "user";
        message.Content = inputField.text;
        Chat chat = new Chat();
        chat.Model = "text-davinci-003";
        chat.Messages = new List<Message> { message };

        inputField.text = "TODO: Replace with AIGC";
        Generating = false;
        return;
    }

    public void GenerateCharacter(GameObject curCharacter, string description)
    {
        /*
        Prompt prompt = new Prompt();
        prompt.PromptText = description;
        StartCoroutine(GenerateCharacterImage(curCharacter, prompt));
        */
        curCharacter.GetComponent<SpriteRenderer>().sprite = CharacterSprite;
        curCharacter.GetComponent<SpriteRenderer>().FadeIn(1.0f);
        Generating = false;
    }

    public void GenerateCharacterStory(TMP_InputField inputField, string description)
    {
        inputField.SetTextWithoutNotify("TODO: Replace with AIGC");
        Generating = false;
    }

    public void GenerateCharacterResponse(TMP_Text text)
    {
        text.text = "TODO: Replace with AIGC";
        Generating = false;
    }

    #region IEnumerators
    IEnumerator GenerateCharacterImage(GameObject curCharacter, Prompt prompt)
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
        StartCoroutine(GetCharacterImage(curCharacter, d.ImageURL));
    }

    IEnumerator GetCharacterImage(GameObject curCharacter, string url)
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
            curCharacter.GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
            curCharacter.GetComponent<SpriteRenderer>().FadeIn(1.0f);
        }

        Generating = false;
    }

    IEnumerator GenerateText(TMP_InputField inputField, Chat chat)
    {
        string requestBody = JsonConvert.SerializeObject(chat);
        byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);

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

        Generating = false;
    }

    #endregion
}
