using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Platinio.UI;

public class DataManager : MonoBehaviour
{
    //Instance
    public static DataManager Instance;

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
        BackgroundStory = "There is a princess \n There is a guard \n There is a prince \n And I'm a sunny happy boy";
        return "There is a princess \n There is a guard \n There is a prince \n And I'm a sunny happy boy";
    }

    public void GenerateCharacter(string description)
    {   
        if (CurCharacter == null)
        {
            CurCharacter = Instantiate(CharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            CurCharacter.transform.parent = GameScene.transform;
            CharacterList.Add(CurCharacter);
            CurCharacter.GetComponent<SpriteRenderer>().FadeIn(1.0f);
        } else
        {
            //TODO: Change the sprite of CurCharacter
        }
    }

    public string GenerateCharacterStory(string description)
    {
        CurCharacter.GetComponent<Character>().CharacterStory = "I'm the king!";
        return "I'm the king!";
    }
}
