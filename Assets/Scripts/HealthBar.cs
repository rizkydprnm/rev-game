using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] Player player;

    void Update() {
        bar.fillAmount = player.playerSave.lives;
    }
}
