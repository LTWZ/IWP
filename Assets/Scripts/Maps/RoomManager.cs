using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int totalEnemies;
    private int defeatedEnemies;
    public bool hasPlayerEntered = false;
    public bool allEnemiesDefeated = false;
    private BoxCollider2D currentBoxcollider;

    public static RoomManager instance;

    public static RoomManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentBoxcollider = GetComponent<BoxCollider2D>();
        OpenExit(); // Start with the exit open
    }

    // Called when an enemy in the room is defeated
    public void EnemyDefeated()
    {
        defeatedEnemies++;

        // Check if all enemies are defeated
        if (defeatedEnemies == totalEnemies)
        {
            // All enemies defeated, allow the player to leave permanently
            OpenExit();
            allEnemiesDefeated = true;
            if (allEnemiesDefeated == true)
            {
                currentBoxcollider.enabled = false;
            }
        }
    }


    // Called when the player enters the room
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (defeatedEnemies == totalEnemies)
            {
                CloseExit(); // Close the exit only if enemies are not all defeated
            }

            hasPlayerEntered = true;
        }
    }

    //// Called when the player exits the room
    //private void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider.CompareTag("Player") && allEnemiesDefeated == true)
    //    {
    //        OpenExit(); // Open the exit when the player exits
    //    }
    //}

    // Open the exit and trigger other relevant actions
    public void OpenExit()
    {
        DisableTilemapCollider.GetInstance().DisableTMCollider();
        TilemapManager.GetInstance().DisableTilemap();
    }

    // Close the exit and trigger other relevant actions
    public void CloseExit()
    {
        DisableTilemapCollider.GetInstance().EnableTMCollider();
        TilemapManager.GetInstance().EnableTilemap();
    }
}