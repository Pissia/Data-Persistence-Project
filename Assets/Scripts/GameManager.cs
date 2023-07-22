using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int _bestScore;
    public string _playerName;
    public string _previousPlayer;
    public int _previousBestScore;

    [SerializeField] TextMeshProUGUI _bestScoreText;
    [SerializeField] TMP_InputField _playerNameInputField;
    
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    private void Start()
    {
        if(_playerName != null)
        {
            _playerNameInputField.text = _playerName;
            _bestScoreText.text = $"Best score: {_playerName} : {_bestScore}";
        }
    }

    public void SetPlayerName()
    {
        if(_playerName != _playerNameInputField.text)
        {
            _previousPlayer = _playerName;
            _previousBestScore = _bestScore;
            _playerName = _playerNameInputField.text;
            _bestScore = 0;
        }
        

        Debug.Log(_playerName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {



#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [Serializable]
    public class PlayerData
    {
        public string name;
        public int bestScore;
    }

    public void SaveData()
    {
        PlayerData data = new PlayerData();
        data.name = _playerName;
        data.bestScore = _bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            _playerName = data.name;
            _bestScore = data.bestScore;
        }
    }
}
