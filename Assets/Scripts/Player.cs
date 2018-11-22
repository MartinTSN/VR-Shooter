using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;
    public GameObject healthBar;
    float currentHealth;

    void Awake()
    {
        currentHealth = health;
    }

    public void FullHealth()
    {
        currentHealth = health;

        Transform pivot = healthBar.transform.Find("HealthyPivot");
        Vector3 scale = pivot.localScale;
        scale.x = Mathf.Clamp(currentHealth / health, 0, 1);

        pivot.localScale = scale;
    }

    public void Hurt(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameObject.Find("Spawner").GetComponent<Spawner>().enabled = false;
            GameObject.Find("Spawner").GetComponent<Spawner>().StopAllCoroutines();
            healthBar.SetActive(false);
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
            RenderSettings.skybox = null;
            GameMenu.currentstate = GameMenu.MenuStates.Lose;
            Thing();
        }

        Transform pivot = healthBar.transform.Find("HealthyPivot");
        Vector3 scale = pivot.localScale;
        scale.x = Mathf.Clamp(currentHealth / health, 0, 1);

        pivot.localScale = scale;
    }

    public void Thing()
    {
        GameObject Rhand = GameObject.Find("RightHand");
        Rhand.GetComponent<LineRenderer>().enabled = true;
        Rhand.GetComponent<Laser>().enabled = true;
        Rhand.GetComponent<MenuInteract>().enabled = true;
        GameObject Lhand = GameObject.Find("LeftHand");
        Lhand.GetComponent<LineRenderer>().enabled = true;
        Lhand.GetComponent<Laser>().enabled = true;
        Lhand.GetComponent<MenuInteract>().enabled = true;
        Rhand.GetComponentInChildren<Attach>().UnSet();
    }
}
