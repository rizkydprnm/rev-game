using UnityEngine;

public class Player : MonoBehaviour
{
    // Collider
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Transform groundDetector;
    [SerializeField] Transform wallDetector;

    // Player
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float maxJumpPower;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float wallSlideSpeed;
    [SerializeField] float wallJumpKnockback;

    // Status player
    bool canExtraJump;
    bool canWallJump;
    bool isWallJumping;

    // Fungsi awal
    void Start()
    {

    }

    // Update dengan sync fisika
    void Update()
    {
        // Gerak kanan kiri
        float direction = Input.GetAxis("Horizontal");
        if (!isWallJumping && direction != 0)
            playerBody.velocity = new Vector2(direction * maxMovementSpeed, playerBody.velocity.y);

        // Lompat
        if (Input.GetButtonDown("Jump"))
        {
            if (IsOnGround() || canExtraJump)
                playerBody.velocity = new Vector2(playerBody.velocity.x, maxJumpPower);
            if (canExtraJump) canExtraJump = false;

            // Wall jump
            if (canWallJump)
            {
                isWallJumping = true;
                if (direction < 0f) playerBody.velocity = new Vector2(wallJumpKnockback, maxJumpPower);
                else if (direction > 0f) playerBody.velocity = new Vector2(-wallJumpKnockback, maxJumpPower);
                Invoke(nameof(StopWallJump), 0.5f);
            }
        }


        // Bisa wall jump?
        if (IsOnWall() && !IsOnGround() && direction != 0f)
        {
            canWallJump = true;
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(-wallSlideSpeed, playerBody.velocity.y));
        }
        else
        {
            canWallJump = false;
            // Batas kecepatan jatuh
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(-maxFallSpeed, playerBody.velocity.y));
        }
    }

    bool IsOnGround()
    {
        return Physics2D.CircleCastAll(groundDetector.position, 0.5f, Vector2.down, 0.01f).Length > 1;
    }

    bool IsOnWall()
    {
        return Physics2D.OverlapBoxAll(wallDetector.position, new Vector2(1.02f, 0.5f), 0).Length > 1;
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }
}

