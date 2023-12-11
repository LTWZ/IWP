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

    private BoxCollider2D boxCollider;
    private bool activatedRoom;

    private int wavesLeft;
    private int amountOfEnemy;
    [SerializeField] private Transform[] spawnPoints;

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

        for (int i = 0; i < amountOfEnemy; i++)
        {
            if (spawnPoints.Length > 0)
            {
                int enemyIndex = Random.Range(0, enemyList.Length);

                // Choose a random spawn point
                int spawnIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[spawnIndex];

                // Instantiate enemy at the chosen spawn point
                GameObject newEnemy = Instantiate(enemyList[enemyIndex], spawnPoint.position, Quaternion.identity);
                newEnemy.GetComponent<EnemyEntity>().SetRoomReference(this);
            }
            else
            {
                // Get the i mean which enemy
                int enemyIndex = Random.Range(0, enemyList.Length);

                // Get the possible position to spawn in
                float xMinPos = boxCollider.bounds.min.x;
                float xMaxPos = boxCollider.bounds.max.x;
                float yMinPos = boxCollider.bounds.min.y;
                float yMaxPos = boxCollider.bounds.max.y;

                float xPos = Random.Range(xMinPos, xMaxPos);
                float yPos = Random.Range(yMinPos, yMaxPos);

                GameObject newEnemy = Instantiate(enemyList[enemyIndex], gameObject.transform);
                newEnemy.transform.position = new Vector3(xPos, yPos, 1);
                newEnemy.GetComponent<EnemyEntity>().SetRoomReference(this);
            }


        }
    }

    /// <summary>
    /// Reduce the enemy count of the room by 1.
    /// </summary>
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

    /// <summary>
    /// Delay the duration at which the wave will spawn.
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayWave()
    {
        yield return new WaitForSeconds(1);
        StartWave();
    }
}
