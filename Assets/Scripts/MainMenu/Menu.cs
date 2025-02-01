using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            int savedLevel = PlayerPrefs.GetInt("SavedLevel");
            SceneManager.LoadScene(savedLevel);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu_Scene");
    }
    public void QuitGame()
    {
        Debug.Log("QUIT! ");
        Application.Quit();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ShowMenu! ");
            ShowMenu();
        }
    }

    void ShowMenu()
    {
        Debug.Log("ShowMenu!22 ");
        SceneManager.LoadScene("Main_Menu_Scene");
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Удаляем все сохраненные данные
        PlayerPrefs.Save();
        Debug.Log("Progress reset!");
    }

}
