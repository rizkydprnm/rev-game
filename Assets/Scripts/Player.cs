using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D compPlayerBody;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float maxJumpPower;
    [SerializeField] float maxFallSpeed;

    // Fungsi awal
    void Start()
    {

    }

    // Update dengan sync fisika
    void Update()
    {
        // Gerak kanan kiri
        float direction = Input.GetAxis("Horizontal");
        compPlayerBody.velocity = new Vector2(direction * maxMovementSpeed, compPlayerBody.velocity.y);

        // Lompat
        if (Input.GetKeyDown(KeyCode.X))
        {
            compPlayerBody.velocity = new Vector2(compPlayerBody.velocity.x, maxJumpPower);
        }

        // Batas kecepatan jatuh
        compPlayerBody.velocity = new Vector2(compPlayerBody.velocity.x, Mathf.Max(-maxFallSpeed, compPlayerBody.velocity.y));
    }
}
