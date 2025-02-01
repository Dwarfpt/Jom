using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SaveCurrentLevel();
    }
    public void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt("SavedLevel", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        Debug.Log("Current level saved: " + SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadSavedLevel()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            int savedLevel = PlayerPrefs.GetInt("SavedLevel");
            SceneManager.LoadScene(savedLevel);
        }
        else
        {
            Debug.LogWarning("No saved level found!");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ShowMenu! ");
            SaveCurrentLevel();
            ShowMenu();
        }
    }

    void ShowMenu()
    {
        Debug.Log("ShowMenu!22 ");
        SceneManager.LoadScene("Main_Menu_Scene");
    }
}
