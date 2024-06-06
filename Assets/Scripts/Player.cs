using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Transform groundDetector;
    [SerializeField] Transform wallDetector;

    [SerializeField, Min(0f)] float maxMovementSpeed;
    [SerializeField, Min(0f)] float maxJumpPower;
    [SerializeField, Min(0f)] float maxFallSpeed;
    [SerializeField, Min(0f)] float acceleration;
    [SerializeField, Min(0f)] float deceleration;

    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator anim;

    bool canExtraJump;
    bool canWallJump;
    bool isWallJumping;

    Vector2 velocity;

    void Start()
    {

    }

    void Update()
    {
        float direction = Input.GetAxis("Horizontal");

        // Buat velocity nya rigid body bisa diedit
        velocity = playerBody.velocity;

        canWallJump = !IsOnGround() && (IsOnWallLeft() || IsOnWallRight());

        if (IsOnGround())
        {
            canExtraJump = true;
        }

        // Batasi fall speed
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        // Input movement kanan kiri
        if (!isWallJumping)
        {

            if (direction != 0f)
            {
                if (Mathf.Sign(velocity.x) != Mathf.Sign(direction) && velocity.x != 0f)
                {
                    velocity.x = Mathf.MoveTowards(
                        velocity.x,
                        0,
                        deceleration * Time.deltaTime
                    );

                }
                else
                {
                    velocity.x = Mathf.MoveTowards(
                        velocity.x,
                        maxMovementSpeed * direction,
                        acceleration * Time.deltaTime
                    );
                }
            }
            else
            {
                velocity.x = Mathf.MoveTowards(
                    velocity.x,
                    0,
                    deceleration * Time.deltaTime
                );
            }
        }

        // Input movement lompat
        if (Input.GetButtonDown("Jump"))
        {
            if (IsOnGround())
            {
                velocity.y = maxJumpPower;
            }
            else if (canExtraJump && !canWallJump)
            {
                canExtraJump = false;
                velocity.y = maxJumpPower;
            }
            else if (canWallJump)
            {
                if (IsOnWallLeft())
                {
                    velocity = new Vector2(maxMovementSpeed, maxJumpPower);
                }
                else if (IsOnWallRight())
                {
                    velocity = new Vector2(-maxMovementSpeed, maxJumpPower);
                }

                isWallJumping = true;
                Invoke(nameof(DisableWallJumping), 0.5f);
            }
        }

        // Kembalikan velocity
        playerBody.velocity = velocity;

        anim.SetBool("Ground", IsOnGround());
        anim.SetFloat("X Speed", Mathf.Abs(playerBody.velocity.x));
        anim.SetFloat("Y Speed", playerBody.velocity.y);

        if (playerBody.velocity.x < 0f) playerSprite.flipX = true;
        else if (playerBody.velocity.x > 0f) playerSprite.flipX = false;
    }

    bool IsOnGround()
    {
        return Physics2D.CircleCastAll(groundDetector.position, 0.5f, Vector2.down, 0.01f).Length > 1;
    }

    bool IsOnWallLeft()
    {
        return Physics2D.BoxCastAll(wallDetector.position, Vector2.one * 0.5f, 0, Vector2.left, 0.55f).Length > 1;
    }

    bool IsOnWallRight()
    {
        return Physics2D.BoxCastAll(wallDetector.position, Vector2.one * 0.5f, 0, Vector2.right, 0.55f).Length > 1;
    }

    void DisableWallJumping()
    {
        isWallJumping = false;
    }
}

