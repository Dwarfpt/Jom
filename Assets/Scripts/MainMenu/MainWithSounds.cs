using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuWithSounds : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip soundClip;        // Ваш звук для кнопок
    public float delayTime = 1.0f;     // Задержка перед выполнением действия после проигрывания звука
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (soundClip == null)
        {
            Debug.LogWarning("No soundClip assigned for MenuWithSound on " + gameObject.name);
        }
    }

    // --- Методы Меню --- //
    public void PlayGame()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
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
        }));
    }

    public void GoToSettingsMenu()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            SceneManager.LoadScene("SettingsMenu");
        }));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            SceneManager.LoadScene("Main_Menu_Scene");
        }));
    }

    public void QuitGame()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            Debug.Log("QUIT!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }));
    }

    public void ResetProgress()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Progress reset!");
        }));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // По ESC возвращаемся в главное меню
            StartCoroutine(PlaySoundThenExecute(() =>
            {
                SceneManager.LoadScene("Main_Menu_Scene");
            }));
        }
    }

    // --- Вспомогательный метод --- //
    private IEnumerator PlaySoundThenExecute(Action action)
    {
        if (soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("No soundClip assigned, executing action immediately.");
        }

        // Ждём фиксированное время (например, длину клипа или немного больше)
        yield return new WaitForSeconds(delayTime);

        action?.Invoke();
    }
}
