using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("New Game");
        SaveSystem.Save(new SaveData());
    }

    public void ContinueGame()
    {
        Debug.Log("Continue");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
