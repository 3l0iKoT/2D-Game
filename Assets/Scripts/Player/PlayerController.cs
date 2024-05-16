using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount;
    [SerializeField] private int airJumps;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCutMult;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float jumpBufferingTime;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feet;
    [SerializeField] private float fallGravityMult;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private float moveInput;
    private float lastGroundTime;
    private float lastJumpTime;
    private bool isJumping = true;
    private bool jumpReleased = true;
    private int bonusJumpLeft;
    private float gravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        gravity = rb.gravityScale;
        bonusJumpLeft = airJumps;
    }

    private void FixedUpdate()
    {
        Run();
        if (lastGroundTime > 0 && Mathf.Abs(moveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        lastGroundTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        moveInput = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = false;
            OnJump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            OnJumpUp();
        }

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }
        
        if (lastGroundTime > 0 && lastJumpTime > 0 && !isJumping)
        {   
            isJumping = true;
            jumpReleased = false;
            Jump();
        }
        else if (lastJumpTime > 0 && bonusJumpLeft > 0)
        {
            isJumping = true;
            jumpReleased = false;

            bonusJumpLeft--;

            Jump();
        }

        if (lastGroundTime > 0 && !isJumping)
        {
            bonusJumpLeft = airJumps;
        }

        SpriteFlip();
        FallGravity();
        CheckGround();
        Animations();
    }

    private void Run()
    {
        float targetSpeed = moveInput * speed;
        anim.SetFloat("horizontalMove", Mathf.Abs(targetSpeed));
        float speedDif = targetSpeed - rb.velocity.x;
        float accRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
    }

    private void Jump()
    {
        float force = jumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        anim.SetTrigger("jump");
        lastGroundTime = 0;
        lastJumpTime = 0;
    }

    private void OnJump()
    {
        lastJumpTime = jumpBufferingTime;
    }

    private void OnJumpUp()
    {
        if (rb.velocity.y > 0 && isJumping)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMult), ForceMode2D.Impulse);
        }
        jumpReleased = true;
        lastJumpTime = 0;
    }

    private void SpriteFlip()
    {
        if (moveInput > 0)
        {
            sprite.flipX = false;
        }
        else if (moveInput < 0)
        {
            sprite.flipX = true;
        }
    }

    private void FallGravity()
    {
        if (rb.velocity.y < 0.0f)
        {
            rb.gravityScale = gravity * fallGravityMult;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    private void CheckGround()
    {
        if (Physics2D.OverlapBox(feet.position, new Vector2(0.49f, 0.03f), 0, groundLayer))
        {
            lastGroundTime = jumpCoyoteTime;
        }
    }

    private void Animations()
    {
        if (rb.velocity.y < 0.1f && lastGroundTime < 0)
        {
            anim.SetBool("isFall", true);
        }
        else
        {
            anim.SetBool("isFall", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(feet.position + new Vector3(-0.49f, -0.03f), feet.position + new Vector3(-0.49f, 0.03f));
        Gizmos.DrawLine(feet.position + new Vector3(-0.49f, 0.03f), feet.position + new Vector3(0.49f, 0.03f));
        Gizmos.DrawLine(feet.position + new Vector3(0.49f, 0.03f), feet.position + new Vector3(0.49f, -0.03f));
        Gizmos.DrawLine(feet.position + new Vector3(0.49f, -0.03f), feet.position + new Vector3(-0.49f, -0.03f));
    }
}
