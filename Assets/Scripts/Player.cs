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

    bool dead;
    bool timeRunning = true;

    float wallJumpingTimeout = 0f;

    Vector2 velocity;
    Vector2 lastCheck;

    public SaveData playerSave = new();

    void Start()
    {
        playerSave = SaveSystem.Load();
    }

    void Update()
    {
        playerSave.currentTime += timeRunning ? Time.deltaTime : 0;

        // Buat velocity nya rigid body bisa diedit
        velocity = playerBody.velocity;

        if (!dead)
        {
            HorizontalMovement();
            VerticalMovement();
            PlayerAnimation();
        }

        // Kembalikan velocity
        playerBody.velocity = velocity;

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

    void HorizontalMovement()
    {
        float direction = Input.GetAxis("Horizontal");

        if (wallJumpingTimeout == 0f)
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
    }

    void VerticalMovement()
    {
        canWallJump = !IsOnGround() && (IsOnWallLeft() || IsOnWallRight());

        if (IsOnGround())
            canExtraJump = true;

        // Batasi fall speed
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

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

                wallJumpingTimeout = 0.25f;
            }
        }

        wallJumpingTimeout = Mathf.MoveTowards(wallJumpingTimeout, 0, Time.deltaTime);
    }

    void PlayerAnimation()
    {
        anim.SetBool("Ground", IsOnGround());
        anim.SetFloat("X Speed", Mathf.Abs(playerBody.velocity.x));
        anim.SetFloat("Y Speed", playerBody.velocity.y);

        if (playerBody.velocity.x < 0f) playerSprite.flipX = true;
        else if (playerBody.velocity.x > 0f) playerSprite.flipX = false;
    }

    void Respawn()
    {
        anim.Play("AnimPlayerIdle");
        transform.position = lastCheck;
        dead = false;
    }

    public void SetCheckpoint(Vector2 checkPosition)
    {
        lastCheck = checkPosition;
    }

    public void Kill()
    {
        anim.Play("AnimPlayerDead");
        Invoke(nameof(Respawn), 1f);
        dead = true;
        playerBody.velocity = Vector2.zero;
    }
}

