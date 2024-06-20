using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalFinish : MonoBehaviour
{
    SaveData playerSave;
    [SerializeField, Min(1)] int nextLevel = 1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().timeRunning = false;
            playerSave = collider.GetComponent<Player>().playerSave;

            playerSave.stage = nextLevel;
            SaveSystem.Save(playerSave);
            
            Destroy(collider);

            Invoke(nameof(NextLevel), 5f);
        }
    }

    void NextLevel()
    {
        SceneManager.LoadSceneAsync(string.Format("Level{0}", playerSave.stage));
    }
}
