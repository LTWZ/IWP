using System.Collections;
using UnityEngine;

public class FinalBossRoom : MonoBehaviour
{
    [SerializeField] GameObject[] doorList;
    [SerializeField] int amountOfWaves = 3;

    [System.Serializable]
    public class EnemyWave
    {
        public GameObject enemyPrefab;
        public int minEnemyPerWave = 2;
        public int maxEnemyPerWave = 4;
    }

    [SerializeField] EnemyWave[] enemyWaves;

    private BoxCollider2D boxCollider;
    private bool activatedRoom;

    private int currentWaveIndex;
    private int amountOfEnemy;

    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        OpenCloseDoors(false);
        activatedRoom = false;
        currentWaveIndex = 0;
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

    void StartWave(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < enemyWaves.Length)
        {
            EnemyWave currentWave = enemyWaves[waveIndex];
            amountOfEnemy = Random.Range(currentWave.minEnemyPerWave, currentWave.maxEnemyPerWave + 1);

            for (int i = 0; i < amountOfEnemy; i++)
            {
                if (spawnPoints.Length > 0)
                {
                    // Choose a random spawn point
                    int spawnIndex = Random.Range(0, spawnPoints.Length);
                    Transform spawnPoint = spawnPoints[spawnIndex];

                    // Instantiate the specific enemy for the current wave at the chosen spawn point
                    GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoint.position, Quaternion.identity);
                    newEnemy.GetComponent<EnemyEntity>().SetRoomReferenceBoss(this);
                }
                else
                {
                    // Get the possible position to spawn in
                    float xMinPos = boxCollider.bounds.min.x;
                    float xMaxPos = boxCollider.bounds.max.x;
                    float yMinPos = boxCollider.bounds.min.y;
                    float yMaxPos = boxCollider.bounds.max.y;

                    float xPos = Random.Range(xMinPos, xMaxPos);
                    float yPos = Random.Range(yMinPos, yMaxPos);

                    // Instantiate the specific enemy for the current wave at the chosen spawn point
                    GameObject newEnemy = Instantiate(currentWave.enemyPrefab, gameObject.transform);
                    newEnemy.transform.position = new Vector3(xPos, yPos, 1);
                    newEnemy.GetComponent<EnemyEntity>().SetRoomReferenceBoss(this);
                }
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
        StartWave(currentWaveIndex);
        currentWaveIndex++;
    }
}