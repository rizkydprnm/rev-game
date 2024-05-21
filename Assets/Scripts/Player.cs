using UnityEngine;

public class Player : MonoBehaviour
{
    // Collider
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Transform groundDetector;
    [SerializeField] Transform leftWallDetector;
    [SerializeField] Transform rightWallDetector;

    // Player
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float maxJumpPower;
    [SerializeField] float maxFallSpeed;

    // Status player
    bool isOnGround, isOnWallLeft, isOnWallRight;
    bool canExtraJump;

    // Fungsi awal
    void Start()
    {

    }

    // Update dengan sync fisika
    void Update()
    {
        // Gerak kanan kiri
        float direction = Input.GetAxis("Horizontal");
        playerBody.velocity = new Vector2(direction * maxMovementSpeed, playerBody.velocity.y);

        // Lompat
        if (Input.GetButtonDown("Jump"))
        {
            if (isOnGround || canExtraJump)
                playerBody.velocity = new Vector2(playerBody.velocity.x, maxJumpPower);
                if (canExtraJump) canExtraJump = false;
        }

        // Batas kecepatan jatuh
        playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(-maxFallSpeed, playerBody.velocity.y));
    }

    void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
    }

    void GroundCheck()
    {
        isOnGround = Physics2D.CircleCastAll(groundDetector.position, 0.5f, Vector2.down, 0.01f).Length > 1;
        if (isOnGround) canExtraJump = true;
    }

    void WallCheck()
    {

    }
}

