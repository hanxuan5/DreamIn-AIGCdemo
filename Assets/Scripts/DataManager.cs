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
using UnityEngine.Playables;
using LitJson;
using System.IO;
using System.Reflection;

public class DataManager : MonoBehaviour
{
    //Instance
    public static DataManager Instance;
    public bool Generating = false;

    //Temp
    public Sprite MapSprite;
    public GameObject CharacterPrefab;
    public GameObject CurCharacter = null;

    //Game Data
    public GameObject Map;
    public GameObject GameScene;
    public string BackgroundStory = "";
    public List<GameObject> CharacterList = new List<GameObject>();

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
        //TODO: 调取API，根据description生成图片
        Map.SetActive(true);
        Map.GetComponent<SpriteRenderer>().sprite = MapSprite;
        Map.GetComponent<SpriteRenderer>().FadeIn(1.0f);

    }

    public string GenerateBackgroundStory(string description)
    {
        BackgroundStory = "TODO: Replace with AIGC";
        return "TODO: Replace with AIGC";
    }

    public void GenerateCharacter(string description)
    {
        Generating = true;
        if (CurCharacter == null)
        {   
            CurCharacter = Instantiate(CharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            CurCharacter.transform.parent = GameScene.transform;
            CharacterList.Add(CurCharacter);

            StartCoroutine(GenerateCharacterImage(description));
        } else
        {
            //TODO: Change the sprite of CurCharacter
        }
    }

    public string GenerateCharacterStory(string description)
    {
        CurCharacter.GetComponent<Character>().CharacterStory = "TODO: Replace with AIGC";
        return "TODO: Replace with AIGC";
    }

    public void GenerateCharacterResponse(TMP_Text response)
    {
        response.text = "TODO: Replace with AIGC";
    }

    #region IEnumerators
    IEnumerator GenerateCharacterImage(string description)
    {
        string url = "http://ai.dreamin.land/api/gen_character/";
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        Encoding encoding = Encoding.UTF8;
        string json = "{" + string.Format("\"prompt\": \"{0}\"", description) + "}";
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
        ImageData d = JsonMapper.ToObject<ImageData>(webRequest.downloadHandler.text);
        StartCoroutine(GetCharacterImage(d.image_url));
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


    #endregion
}
