using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject[] doorList;
    [SerializeField] GameObject[] enemyList;
    [SerializeField] int amountOfWaves = 3;
    [SerializeField] int minEnemyPerWave = 2;
    [SerializeField] int maxEnemyPerWave = 4;
    [SerializeField] GameObject spawnIndicatorPrefab;

    private BoxCollider2D boxCollider;
    private bool activatedRoom;
    private int wavesLeft;
    private int amountOfEnemy;
    [SerializeField] Transform[] spawnPoints;

    private void Start()
    {
        wavesLeft = 3;
        OpenCloseDoors(false);
        activatedRoom = false;
        amountOfEnemy = 0;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activatedRoom)
            return;

        if (other.GetComponent<PlayerEntity>())
        {
            OpenCloseDoors(true);
            StartCoroutine(DelayWave());
            activatedRoom = true;
        }
    }

    void OpenCloseDoors(bool doorActive)
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            doorList[i].SetActive(doorActive);
        }
    }

    void StartWave()
    {
        amountOfEnemy = Random.Range(minEnemyPerWave, maxEnemyPerWave + 1);

        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < amountOfEnemy; i++)
        {
            if (availableSpawnPoints.Count > 0)
            {
                int enemyIndex = Random.Range(0, enemyList.Length);

                // Choose a random spawn point from the available options
                int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[spawnIndex];

                // Remove the chosen spawn point from the available options
                availableSpawnPoints.RemoveAt(spawnIndex);

                // Instantiate spawn indicator at the chosen spawn point
                GameObject spawnIndicator = Instantiate(spawnIndicatorPrefab, spawnPoint.position, Quaternion.identity);

                // Instantiate enemy after a delay
                StartCoroutine(SpawnEnemyWithDelay(enemyIndex, spawnPoint.position, spawnIndicator));
            }
            else
            {
                // Handle the case when there are no more available spawn points
                Debug.LogWarning("No more available spawn points.");
                break;
            }
        }
    }

    IEnumerator SpawnEnemyWithDelay(int enemyIndex, Vector3 spawnPoint, GameObject spawnIndicator)
    {
        // Wait for a random delay between 0.5 to 1 second
        float delay = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(delay);

        // Destroy the spawn indicator
        Destroy(spawnIndicator);

        // Instantiate enemy at the chosen spawn point
        GameObject newEnemy = Instantiate(enemyList[enemyIndex], spawnPoint, Quaternion.identity);
        newEnemy.GetComponent<EnemyEntity>().SetRoomReference(this);
    }

    public void ReduceEnemy()
    {
        amountOfEnemy--;

        if (amountOfEnemy <= 0)
        {
            amountOfWaves--;

            if (amountOfWaves > 0)
            {
                StartCoroutine(DelayWave());
            }
            else
            {
                OpenCloseDoors(false);
            }
        }
    }

    IEnumerator DelayWave()
    {
        yield return new WaitForSeconds(1);
        StartWave();
    }
}