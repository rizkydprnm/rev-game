using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector2.zero;
    }

    // Update is called once per frame
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
        SaveSystem.Save(GameObject.Find("Player").GetComponent<Player>().playerSave);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
