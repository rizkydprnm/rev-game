using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Collider
    [SerializeField] Rigidbody2D compPlayerBody;
    [SerializeField] BoxCollider2D compGroundDetector;
    [SerializeField] BoxCollider2D compLeftWallDetector, compRightWallDetector;

    // Player
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float maxJumpPower;
    [SerializeField] float maxFallSpeed;

    // Status player
    bool isOnGround;
    bool isOnWallLeft, isOnWallRight;
    float wallTimer = 5.0f;

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
        if (Input.GetButtonDown("Jump") && isOnGround)
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
        // Unity Insanity 1: kenapa harus declare collider dulu
        RaycastHit2D[] colliders = new RaycastHit2D[2];
        isOnGround = compGroundDetector.Cast(Vector2.zero, colliders) > 0;
    }

    void WallCheck()
    {
        RaycastHit2D[] leftColliders = new RaycastHit2D[2];
        RaycastHit2D[] rightColliders = new RaycastHit2D[2];

        isOnWallLeft = compLeftWallDetector.Cast(Vector2.zero, leftColliders) > 1;
        isOnWallRight = compRightWallDetector.Cast(Vector2.zero, rightColliders) > 1;

        // Jika nempel tembok tapi gak ditanah
        if ((isOnWallLeft || isOnWallRight) && !isOnGround)
        {
            wallTimer -= Time.deltaTime;
        }
        else wallTimer = 5.0f;

        // TODO: Jika timer tembok masih, nyangkut ditembok
        if (wallTimer > 0) {

        }
        else {

        }
    }
}
