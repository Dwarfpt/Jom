using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Touching_Directions), typeof(Damageable))]
// [RequireComponent(typeof(Rigidbody2D), typeof(Touching_Directions))]
public class Knight : MonoBehaviour
{

    public float walkStopRate = 0.6f;
    public float walkSpeed = 3f;
    public float jumpForce = 5f; // Для прыжка, если потребуется

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    private Rigidbody2D rb;
    private Touching_Directions touchingDirection;

    private Animator animator;
    Damageable damageable;


    public enum WalkDirection
    {
        Right, //Right
        Left //Left
    }

    private WalkDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right; //right
    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }


    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public WalkDirection CurrentWalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Flip direction
                transform.localScale = new Vector2(
                    Mathf.Abs(transform.localScale.x) * (value == WalkDirection.Right ? 1 : -1),//Right
                    transform.localScale.y
                );

                walkDirectionVector = value == WalkDirection.Right ? Vector2.right : Vector2.left;//WalkDirection.Right ? Vector2.right : Vector2.left;
                _walkDirection = value;
            }
        }
    }

    //public Damageable damageable { get => damageable; set => damageable = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<Touching_Directions>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        // Если герой должен начать двигаться вправо, задайте начальный localScale
        if (CurrentWalkDirection == WalkDirection.Right)//CurrentWalkDirection == WalkDirection.Right
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        if (animator == null)
        {
            Debug.LogError("Animator не найден на объекте: " + gameObject.name);
        }
    }

    void Update()
    {
        //  Debug.Log("Update вызван для: " + gameObject.name);
        if (attackZone == null)
        {
            Debug.LogError("AttackZone не назначен для: " + gameObject.name);
            return;
        }
        HasTarget = attackZone.detectedColliders.Count > 0;
    }
    private void FixedUpdate()
    {

        // if (touchingDirection.IsOnWall && touchingDirection.IsGrounded || cliffDetectionZone.detectedColliders.Count == 0)
        if (touchingDirection.IsOnWall && touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
        // if (CanMove)
        // {
        //     rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        // else
        //     {
        //         rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        //     }
        // }
        if (!damageable.LockVelocity)
        {
            if (canMove)
            {
                // Враг двигается только по земле
                if (touchingDirection.IsGrounded)
                {
                    rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
                }
                else
                {
                    // Добавить логику для движения в воздухе, если необходимо
                    rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
                }
            }
        }
    }


    private void FlipDirection()
    {
        CurrentWalkDirection = CurrentWalkDirection == WalkDirection.Right
            ? WalkDirection.Left
            : WalkDirection.Right;
        // Исправление масштаба
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * (walkDirectionVector.x > 0 ? 1 : -1), transform.localScale.y);
    }

    //        private void FlipDirection()
    // {
    //     CurrentWalkDirection = CurrentWalkDirection == WalkDirection.Right
    //         ? WalkDirection.Left
    //         : WalkDirection.Right;
    //     // Исправление масштаба
    //     transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * (walkDirectionVector.x > 0 ? 1 : -1), transform.localScale.y);
    // }


    public void OnHit(int damage, Vector2 knockback)
    {
        //LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if(touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }
}
