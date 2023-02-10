using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSON : MonoBehaviour
{
    public Data playerData = new Data();
    private bool Ismobile;
    public static JSON instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    private void Start()
    {
#if UNITY_ANDROID
        {
            Ismobile = true;
        }
#endif

#if UNITY_EDITOR
        {
            Ismobile = false;
        }
#endif
        LoadPlayerDataToJson();
        SceneManager.LoadScene(playerData.currentStage);
    }

    [ContextMenu("To Json Data")]
    public void SavePlayerDataToJson()
    {
        playerData.currentStage = SceneManager.GetActiveScene().buildIndex;
        string JsonData = JsonUtility.ToJson(playerData, true);

        string path;
        if (!Ismobile)
        {
            path = Path.Combine(Application.dataPath, "PlayerData.json");
        }
        else
        {
            path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        }


        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(JsonData);
        string code = System.Convert.ToBase64String(bytes);

        File.WriteAllText(path, code);
        Debug.Log(code);
    }

    [ContextMenu("Load Json Data")]
    public void LoadPlayerDataToJson()
    {
        string path;

        if (!Ismobile)
        {
            path = Path.Combine(Application.dataPath, "PlayerData.json");
        }
        else
        {
            path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        }

        string jsonData = File.ReadAllText(path);

        byte[] bytes = System.Convert.FromBase64String(jsonData);
        string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        playerData = JsonUtility.FromJson<Data>(jdata);
    }
}

[System.Serializable]
public class Data
{
    public int currentStage;
}