using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string enemyName;
        public GameObject enemyPrefab;
    }

    [System.Serializable]
    public class Room
    {
        public string roomName;
        public List<Transform> spawnPoints;
        public List<EnemyType> enemyTypes;
        public GameObject door; // Reference to the door associated with the room
    }

    public List<Room> rooms = new List<Room>();

    public static RoomManager instance;

    public static RoomManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Example: Spawn enemies in the first room
        SpawnEnemies("Room1");
    }

    public void SpawnEnemies(string roomName)
    {
        Room room = GetRoomByName(roomName);

        if (room != null)
        {
            foreach (Transform spawnPoint in room.spawnPoints)
            {
                EnemyType enemyType = room.enemyTypes[Random.Range(0, room.enemyTypes.Count)];
                Instantiate(enemyType.enemyPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Room not found: " + roomName);
        }
    }

    private Room GetRoomByName(string roomName)
    {
        return rooms.Find(room => room.roomName == roomName);
    }

    public void EnemyDefeated(string roomName)
    {
        Room room = GetRoomByName(roomName);

        if (room != null)
        {
            // Check if all enemies are defeated in the room
            if (AllEnemiesDefeated(room))
            {
                OpenDoor(room);
            }
        }
        else
        {
            Debug.LogWarning("Room not found: " + roomName);
        }
    }

    private bool AllEnemiesDefeated(Room room)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Adjust the tag as needed

        foreach (Transform spawnPoint in room.spawnPoints)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.transform.position == spawnPoint.position)
                {
                    return false; // At least one enemy is still alive in the room
                }
            }
        }

        return true; // All enemies are defeated in the room
    }

    private void OpenDoor(Room room)
    {
        if (room.door != null)
        {
            // Add logic to open the door (e.g., set the door to active)
            room.door.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Door not found for room: " + room.roomName);
        }
    }
}