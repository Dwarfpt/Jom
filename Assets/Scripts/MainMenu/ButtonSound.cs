using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSound : MonoBehaviour
{
    public AudioClip soundClip; // Звук для этой конкретной кнопки
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (soundClip == null)
        {
            Debug.LogWarning("SoundClip not assigned for " + gameObject.name);
        }
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Просто проиграть звук (без последствий)
    /// </summary>
    public void PlaySound()
    {
        if (soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("No sound assigned on " + gameObject.name);
        }
    }

    /// <summary>
    /// Проиграть звук, затем загрузить указанную сцену
    /// </summary>
    public void PlaySoundAndLoadScene(string sceneName)
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }

    /// <summary>
    /// Проиграть звук, затем загрузить сохраненный уровень или следующий по индексу
    /// </summary>
    public void PlaySoundAndLoadGame()
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

    /// <summary>
    /// Проиграть звук, затем выйти из приложения
    /// </summary>
    public void PlaySoundAndQuit()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }));
    }

    /// <summary>
    /// Проиграть звук, затем сбросить прогресс
    /// </summary>
    public void PlaySoundAndResetProgress()
    {
        StartCoroutine(PlaySoundThenExecute(() =>
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Progress reset!");
        }));
    }

    private IEnumerator PlaySoundThenExecute(System.Action action)
    {
        if (soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
            yield return new WaitForSeconds(soundClip.length);
        }
        action?.Invoke();
    }
}
