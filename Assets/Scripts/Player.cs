using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] AudioSource jumpSound;

    bool canExtraJump;
    bool canWallJump;

    bool dead;

    float wallJumpingTimeout = 0f;

    Vector2 velocity;
    Vector2 lastCheck;

    [HideInInspector] public bool timeRunning = true;
    [HideInInspector] public SaveData playerSave = new();

    void Start()
    {
        playerSave = SaveSystem.Load();
        lastCheck = transform.position;
    }

    void Update()
    {
        playerSave.time += timeRunning ? Time.deltaTime : 0;

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
                jumpSound.Play();
                velocity.y = maxJumpPower;
            }
            else if (canExtraJump && !canWallJump)
            {
                jumpSound.Play();
                canExtraJump = false;
                velocity.y = maxJumpPower;
            }
            else if (canWallJump)
            {
                if (IsOnWallLeft())
                {
                    jumpSound.Play();
                    velocity = new Vector2(maxMovementSpeed, maxJumpPower);
                }
                else if (IsOnWallRight())
                {
                    jumpSound.Play();
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
        dead = false;
        anim.Play("AnimPlayerIdle");
        transform.position = lastCheck;
    }

    public void SetCheckpoint(Vector2 checkPosition)
    {
        lastCheck = checkPosition;
    }

    public void Kill()
    {
        if (!dead)
        {
            dead = true;
            anim.Play("AnimPlayerDead");
            playerBody.velocity = Vector2.zero;

            playerSave.lives = Mathf.MoveTowards(playerSave.lives, 0, 0.25f);

            if (playerSave.lives > 0f)
                Invoke(nameof(Respawn), 1f);
            else
            {
                timeRunning = false;
                Invoke(nameof(Restart), 2f);
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        playerSave.lives = 5f;
    }
}

