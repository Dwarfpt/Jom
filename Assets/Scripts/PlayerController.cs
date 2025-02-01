using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Rigidbody2D), typeof(Touching_Directions), typeof(Damageable))]
// [RequireComponent(typeof(Rigidbody2D), typeof(Touching_Directions))]

public class PlayerController : MonoBehaviour
{
    public float airWalkSpeed = 2f;
    public float jumpImpulse = 10f;
    Touching_Directions touchingDirections;
    public float walkSpeed = 10f;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    private bool _IsMoving = false;
    public bool _isFacingRight = true;
    public float CurrentMoveSpeed
    {
        get
        {
            if (canMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        return walkSpeed;
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField]
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    [SerializeField]
    public bool IsMoving
    {
        get
        {
            return _IsMoving;
        }
        private set
        {
            _IsMoving = value;
            //animator.SetBool("IsMoving", value);
            animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<Touching_Directions>();
        damageable = GetComponent<Damageable>();
    }
    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput.x * walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
        //rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);
        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    // public bool LockVelocity
    // {
    //     get
    //     {
    //         return animator.GetBool(AnimationStrings.LockVelocity);

    //     }
    //     set
    //     {
    //         animator.SetBool(AnimationStrings.LockVelocity, value);
    //     }
    // }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsAlive && canMove)
        {
            if (context.started && touchingDirections.IsGrounded)
            {
                animator.SetTrigger(AnimationStrings.jumpTrigger);
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            }
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnSpeacialAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.specialAttackTrigger);
        }
    }
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.IsAlive);
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        //damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}

