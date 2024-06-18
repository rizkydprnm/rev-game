using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Animator anim;
    bool passed;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !passed)
        {
            passed = true;
            anim.Play("AnimCheckRise");
            collider.gameObject.GetComponent<Player>().SetCheckpoint(transform.position);
        }
    }
}
