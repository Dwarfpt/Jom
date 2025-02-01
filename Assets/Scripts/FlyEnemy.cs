using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    public float flightSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;
    
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    Transform nextWaypoint;
    int waypointNum = 0;
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

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();

    }

    void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;

    }

    public void FixedUpdate()
    {
        if(damageable.IsAlive)
        {
            if(canMove)
            {
                Flight();
            }else
            {
                rb.velocity = Vector3.zero;
            }
        }else
        {
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    public void Flight()
    {

        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        if(distance <= waypointReachedDistance)
        {
            waypointNum++;
            if(waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }
            nextWaypoint = waypoints[waypointNum];
        }
    }

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public void UpdateDirection()
    {
        Vector3  locScale = transform.localScale;

        if(transform.localScale.x > 0)
        {
            if(rb.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
                }
        }else
        {
                if(rb.velocity.x > 0)
                {
                    transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
                }
        }
    }
}

