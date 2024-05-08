using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Collider
    [SerializeField] Rigidbody2D compPlayerBody;
    [SerializeField] BoxCollider2D compGroundDetector;
    [SerializeField] BoxCollider2D compWallDetector;

    // Player
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float maxJumpPower;
    [SerializeField] float maxFallSpeed;

    // Status player
    bool isOnGround;
    bool isOnWall;


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
        if (Input.GetKeyDown(KeyCode.X) && isOnGround)
        {
            compPlayerBody.velocity = new Vector2(compPlayerBody.velocity.x, maxJumpPower);
        }

        // Batas kecepatan jatuh
        compPlayerBody.velocity = new Vector2(compPlayerBody.velocity.x, Mathf.Max(-maxFallSpeed, compPlayerBody.velocity.y));
    }

    void FixedUpdate()
    {
        WallCheck();
        GroundCheck();
    }

    // Deteksi tanah
    void GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapArea(compGroundDetector.bounds.min, compGroundDetector.bounds.max);
        isOnGround = collider.gameObject.CompareTag("Ground");
    }

    void WallCheck()
    {

    }
}
