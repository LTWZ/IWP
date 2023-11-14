using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int totalEnemies;
    private int defeatedEnemies;

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
        OpenExit();
    }

    // Called when an enemy in the room is defeated
    public void EnemyDefeated()
    {
        defeatedEnemies++;

        // Check if all enemies are defeated
        if (defeatedEnemies == totalEnemies)
        {
            // All enemies defeated, allow the player to leave
            OpenExit();
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        DisableTilemapCollider.GetInstance().EnableTMCollider();
    //        TilemapManager.GetInstance().EnableTilemap();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        DisableTilemapCollider.GetInstance().DisableTMCollider();
    //        TilemapManager.GetInstance().DisableTilemap();
    //    }
    //}

    // Called when the player enters the room
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && defeatedEnemies != totalEnemies)
        {
            CloseExit();
        }
    }

    // Called when the player exits the room
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (defeatedEnemies == totalEnemies)
            {
                OpenExit();
            }
            else
            {

            }
        }
    }

    // Close the exit or trigger other relevant actions
    public void CloseExit()
    {
        DisableTilemapCollider.GetInstance().EnableTMCollider();
        TilemapManager.GetInstance().EnableTilemap();
    }

    // Open the exit or trigger other relevant actions
    public void OpenExit()
    {
        DisableTilemapCollider.GetInstance().DisableTMCollider();
        TilemapManager.GetInstance().DisableTilemap();
    }
}
