using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The prefab of the enemy to spawn
    public int numberOfEnemiesToSpawn = 5;  // Number of enemies to spawn
    public float spawnInterval = 2f;
    public float spawnRadius = 5f; // Time interval between spawns
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
        // Generate a random position within the specified radius
        Vector2 randomSpawnPosition = (Random.insideUnitCircle * spawnRadius) + (Vector2)transform.position;

        // Instantiate the enemy at the random position
        Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
    }
}
