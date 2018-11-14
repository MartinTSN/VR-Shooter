using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;

    float currentHealth;

    void Awake()
    {
        currentHealth = health;
    }

    public void Hurt(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
            thing();
            RenderSettings.skybox = null;
            GameMenu.currentstate = GameMenu.MenuStates.Lose;
        }
    }

    public void thing()
    {
        GameObject Rhand = GameObject.Find("RightHand");
        Rhand.GetComponent<LineRenderer>().enabled = true;
        Rhand.GetComponent<Laser>().enabled = true;
        Rhand.GetComponent<MenuInteract>().enabled = true;
        GameObject Lhand = GameObject.Find("LeftHand");
        Lhand.GetComponent<LineRenderer>().enabled = true;
        Lhand.GetComponent<Laser>().enabled = true;
        Lhand.GetComponent<MenuInteract>().enabled = true;
        Rhand.GetComponentInChildren<Attatch>().UnSet();
    }
}
