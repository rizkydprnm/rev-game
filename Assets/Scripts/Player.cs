using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D compPlayerBody;

    // Fungsi awal
    void Start()
    {
        
    }

    // Update dengan sync fisika
    void FixedUpdate()
    {
        // Gravitasi
        compPlayerBody.velocity += Time.deltaTime * Physics2D.gravity;

        float direction = Input.GetAxis("Horizontal");
    }
}
