using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public GameObject healthBar;
    //public Slider healthBarSlider;
    float currentHealth;

    void Awake()
    {
        currentHealth = health;

        //healthBarSlider.maxValue = health;

        //if (GameObject.Find("VRCamera") != null)
        //{
        //    healthBarSlider.GetComponentInParent<Canvas>().worldCamera = GameObject.Find("VRCamera").GetComponent<Camera>();
        //}
        //if (GameObject.Find("FallbackObjects") != null)
        //{
        //    healthBarSlider.GetComponentInParent<Canvas>().worldCamera = GameObject.Find("FallbackObjects").GetComponent<Camera>();
        //}
    }

    public void FullHealth()
    {
        currentHealth = health;

        //healthBarSlider.value = currentHealth;

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
            //healthBarSlider.value = 0;
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

        //healthBarSlider.value = currentHealth;

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
