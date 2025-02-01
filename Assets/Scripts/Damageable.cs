using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    // public bool IsHit
    // {
    //     get
    //     {
    //         return animator.GetBool(AnimationStrings.IsHit);
    //     }
    //     private set
    //     {
    //         animator.SetBool(AnimationStrings.IsHit, value);
    //     }
    // }
    public UnityEvent<int, int> healhChanged;
    public UnityEvent<int, Vector2> damageableHit;

    private Animator animator;
    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            healhChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    private float timeSinceHit = 0;
    public float invulnerableTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool("IsAlive", _isAlive);
            Debug.Log("IsAlive set to: " + value);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.LockVelocity);

        }
        set
        {
            animator.SetBool(AnimationStrings.LockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [SerializeField]
    private bool isInvincible = false;

    public void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invulnerableTime)
            {
                // Remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }

        //Hit(10); // пример использования
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            Debug.Log("Damage taken: " + damage + " | Health left: " + Health);
            isInvincible = true;
            //IsHit = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;

        }
        return false;
    }
}