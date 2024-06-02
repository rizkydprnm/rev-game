using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Collider
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Transform groundDetector;
    [SerializeField] Transform wallDetector;

    // Player
    [SerializeField, Min(0f)] float maxMovementSpeed;
    [SerializeField, Min(0f)] float maxJumpPower;
    [SerializeField, Min(0f)] float maxFallSpeed;
    [SerializeField, Min(0f)] float acceleration;
    [SerializeField, Min(0f)] float deceleration;

    // Status player
    bool canExtraJump;
    bool canWallJump;
    bool isWallJumping;

    Vector2 velocity;

    // Fungsi awal
    void Start()
    {

    }

    // Main loop
    void Update()
    {
        // Cek input
        float direction = Input.GetAxis("Horizontal");

        // Buat velocity nya rigid body bisa diedit
        velocity = playerBody.velocity;

        // Bisa wall jump?
        canWallJump = !IsOnGround() && IsOnWall() && direction != 0f;

        // Double jump aktif saat menyentuh tanah
        if (IsOnGround()) canExtraJump = true;

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
                velocity = new Vector2(maxMovementSpeed * Mathf.Sign(-direction), maxJumpPower);
                isWallJumping = true;
                Invoke(nameof(DisableWallJumping), 0.5f);
            }
        }

        // Kembalikan velocity
        playerBody.velocity = velocity;
    }

    bool IsOnGround()
    {
        return Physics2D.CircleCastAll(groundDetector.position, 0.5f, Vector2.down, 0.01f).Length > 1;
    }

    bool IsOnWall()
    {
        return Physics2D.OverlapBoxAll(wallDetector.position, new Vector2(1.02f, 0.5f), 0).Length > 1;
    }

    void DisableWallJumping()
    {
        isWallJumping = false;
    }
}

