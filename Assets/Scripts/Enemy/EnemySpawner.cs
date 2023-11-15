using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The prefab of the enemy to spawn
    public int numberOfEnemiesToSpawn = 5;  // Number of enemies to spawn
    public float spawnInterval = 2f;  // Time interval between spawns
    private Coroutine coroutine;
    void Update()
    {
        if (RoomManager.GetInstance().hasPlayerEntered == true && coroutine == null)
        {
            coroutine = StartCoroutine(SpawnEnemies());
        }
    }
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
            if (RoomManager.GetInstance() != null)
            {
                RoomManager.GetInstance().totalEnemies++;
            }
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
