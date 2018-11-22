/*

            Handles all logic for enemies.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A script that is put on an enemy.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    /// <summary>
    /// The prefab of the healthbar object.
    /// </summary>
    public GameObject healthBarPrefab;
    /// <summary>
    /// The Healthbar object.
    /// </summary>
    GameObject healthBar;

    /// <summary>
    /// The amount of Health that the enemy has.
    /// </summary>
    public float health = 20f;
    /// <summary>
    /// The amount of health that the enemy currently has.
    /// </summary>
    float currentHealth;
    /// <summary>
    /// How much damage the enemy does.
    /// </summary>
    public float damage = 5f;
    /// <summary>
    /// The Target for the enemy.
    /// </summary>
    public Transform target;
    /// <summary>
    /// How fast the enemy moves.
    /// </summary>
    public float moveSpeed = 1f;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    Material skinMaterial;
    Color originalColor;

    protected bool dead;
    public event System.Action OnDeath;

    public GameObject deathEffect;

    NavMeshAgent pathfinder;

    void Start()
    {
        gameObject.tag = "Enemy";
        originalColor = skinMaterial.color;

        currentState = State.Chasing;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<SphereCollider>().radius;
        StartCoroutine(UpdatePath());
    }

    public void SetCharacteristics(float movespeed, float Damage, float Health, Transform Target)
    {
        pathfinder.speed = movespeed;
        damage = Damage;
        health = Health;
        target = Target;
    }

    void Awake()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;

        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, transform);
        healthBar.transform.Rotate(90, 0, 0);
        healthBar.SetActive(false);
    }

    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
            if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;

        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * myCollisionRadius;


        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }
        target.gameObject.GetComponent<Player>().Hurt(damage);
        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.15f;
        while (target != null)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                pathfinder.SetDestination(targetPosition);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    /// <summary>
    /// Used when the enemy is hit.
    /// </summary>
    /// <param name="damage">How much damage the enemy takes.</param>
    public void Hurt(float damage)
    {
        healthBar.SetActive(true);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 1.5f);
            dead = true;
            if (OnDeath != null)
            {
                OnDeath();
            }
            Destroy(gameObject);
        }

        Transform pivot = healthBar.transform.Find("HealthyPivot");
        Vector3 scale = pivot.localScale;
        scale.x = Mathf.Clamp(currentHealth / health, 0, 1);

        pivot.localScale = scale;
    }
}
