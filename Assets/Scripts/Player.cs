using System;
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
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;

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
        // Buat velocity nya Rigid body bisa diedit
        velocity = playerBody.velocity;

        // Double jump aktif saat menyentuh tanah
        if (IsOnGround()) canExtraJump = true;

        // Handle jump
        if (Input.GetButtonDown("Jump"))
        {
            if (IsOnGround())
            {
                velocity.y = maxJumpPower;
            }
            else if (canExtraJump)
            {
                canExtraJump = false;
                velocity.y = maxJumpPower;
            }
        }

        // Input
        float direction = Input.GetAxis("Horizontal");
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
        else {
            velocity.x = Mathf.MoveTowards(
                velocity.x,
                0,
                deceleration * Time.deltaTime
            );
        }

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
}

