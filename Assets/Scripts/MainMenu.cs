using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SaveSystem.Save(new SaveData());
        SceneManager.LoadSceneAsync("Level1");
    }

    public void ContinueGame()
    {
        SaveData data = SaveSystem.Load();
        SceneManager.LoadSceneAsync(string.Format("Level{0}", data.stage));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
