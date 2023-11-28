using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject[] doorList;
    [SerializeField] GameObject[] enemyList; //LeroyIs cute
    [SerializeField] int howManyEnemyToSpawn = 1;
    private BoxCollider2D boxCollider;
    private bool activatedRoom;
    private int amountOfEnemy;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        OpenCloseDoors(false);
        activatedRoom = false;
        amountOfEnemy = 0;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Im called");
        if (activatedRoom)
            return;

        if (other.GetComponent<PlayerEntity>())
        {
            OpenCloseDoors(true);
        }
        SpawnEnemy();
        activatedRoom = true;
    }

    void OpenCloseDoors(bool doorActive)
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            doorList[i].SetActive(doorActive);
        }
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < howManyEnemyToSpawn; i++)
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
                amountOfEnemy++;
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
                amountOfEnemy++;
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
            OpenCloseDoors(false);
        }
    }
}
