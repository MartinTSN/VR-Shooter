using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Wave[] Waves;
    public Enemy enemy;

    public Text enemiesRemainingText;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    Generator map;

    void Start()
    {
        map = FindObjectOfType<Generator>();
        NextWave();
    }

    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.TimeBetweenSpawns;

            StartCoroutine(SpawnEnemy());
        }
        enemiesRemainingText.text = enemiesRemainingAlive.ToString();
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform randomtile = map.GetRandomOpenTile();
        Material tilemat = randomtile.GetComponent<Renderer>().material;
        Color initialcolor = tilemat.color;

        Color flashcolor = Color.red;
        float spawntimer = 0;

        while (spawntimer < spawnDelay)
        {
            tilemat.color = Color.Lerp(initialcolor, flashcolor, Mathf.PingPong(spawntimer * tileFlashSpeed, 1));

            spawntimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, randomtile.position + Vector3.up, Quaternion.identity) as Enemy;
        tilemat.color = initialcolor;
        spawnedEnemy.OnDeath += onEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.damage, currentWave.health, currentWave.target);
    }

    void onEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            if (GameObject.Find("AssaultGun") == true)
            {
                GameObject.Find("AssaultGun").GetComponentInParent<Attach>().Reload();
            }
            else if (GameObject.Find("HandGun") == true)
            {
                GameObject.Find("HandGun").GetComponentInParent<Attach>().Reload();
            }
            GameMenu.currentstate = GameMenu.MenuStates.Wave;
            NextWave();
            gameObject.GetComponent<Spawner>().enabled = false;
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < Waves.Length)
        {
            currentWave = Waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.EnemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
        else
        {
            GameMenu.currentstate = GameMenu.MenuStates.Won;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int EnemyCount;
        public float TimeBetweenSpawns;

        public float moveSpeed;
        public float damage;
        public float health;
        public Transform target;
    }
}
