using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chatgpt : MonoBehaviour
{
    private string apiKey = "org-d1tRV2MPvc9BSdyMsHYuaFt3";
    private string url = "https://api.openai.com/v1/chat/completions";

    void Start()
    {
        StartCoroutine(SendRequest());
    }

    IEnumerator SendRequest()
    {
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[] {
                new {
                    role = "user",
                    content = "What is the OpenAI mission?"
                }
            }
        };

        var jsonBody = JsonUtility.ToJson(requestBody);

        using (var webRequest = UnityWebRequest.Post(url, jsonBody))
        {
            webRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"Request failed with error: {webRequest.error}");
            }
        }
    }
}
