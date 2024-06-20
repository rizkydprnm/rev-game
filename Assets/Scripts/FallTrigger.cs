using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement ; 

public class FallTrigger : MonoBehaviour
{
    [SerializeField]
    Transform Player ;


    private void Update()
    {
        transform.position = new Vector2(Player.position.x, transform.position.y);
    }


 
}
