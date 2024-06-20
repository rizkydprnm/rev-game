using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {
        transform.localScale = Vector2.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        transform.localScale = Vector2.one;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        transform.localScale = Vector2.zero;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
