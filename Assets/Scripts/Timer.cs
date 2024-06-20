using System; 
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Player player;

    // Update is called once per frame
    void Update()
    {
        SaveData playerSave = player.playerSave;
        TimeSpan timeString = TimeSpan.FromSeconds(playerSave.time);
        timerText.text = timeString.ToString(@"m\:ss\.ff");
    }
}
