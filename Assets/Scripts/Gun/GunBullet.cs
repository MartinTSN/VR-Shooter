﻿/*

            Handles all Bullet logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the bullet.
/// </summary>
public class GunBullet : MonoBehaviour
{
    /// <summary>
    /// How fast the bullet is.
    /// </summary>
    public float speed = 3f;
    /// <summary>
    /// How much damage the bullet does.
    /// </summary>
    public float damage = 1f;
    /// <summary>
    /// How long it takes for the bullet to despawn. 1f = 1 sec.
    /// </summary>
    [Tooltip("1 = 1 second")]
    public float bulletDespawnRate = 4f;
    /// <summary>
    /// A tempoary rigidbody.
    /// </summary>
    Rigidbody tempBody;
    public GameObject deathEffect;

    void Start()
    {
        tempBody = gameObject.GetComponent<Rigidbody>();
        tempBody.AddForce(GunShoot.raycast.direction * speed);
    }

    void Update()
    {
        Destroy(gameObject, bulletDespawnRate);
    }

    public void SetCharacteristics(float Speed, float Damage)
    {
        speed = Speed;
        damage = Damage;
    }

    /// <summary>
    /// Checks if the bullet hit an enemy.
    /// </summary>
    /// <param name="other">A gameobjects collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Enemy")
        {
            obj.SendMessage("Hurt", damage);
            Destroy(gameObject);
        }
        if (obj.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks if the bullet hit something other than an enemy.
    /// </summary>
    /// <param name="collision">The gameobject that is hit</param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Ground")
        {
            Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 0.75f);
            Destroy(gameObject);
        }
        if (obj.tag == "Object")
        {
            if (obj.name == "Poplar_Tree")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 0.75f);
                Destroy(gameObject);
            }
        }
        if (obj.tag == "Wall")
        {
            Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 0.75f);
            Destroy(gameObject);
        }
    }
}
