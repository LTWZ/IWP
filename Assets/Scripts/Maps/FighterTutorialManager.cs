using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterTutorialManager : MonoBehaviour
{
    public int expectedLayer = 10; // Set the expected layer for this room
    public TilemapManager tilemapManager;
    public DisableTilemapCollider disableTilemapCollider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.gameObject.layer == expectedLayer)
        {
            OpenRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.gameObject.layer == expectedLayer)
        {
            CloseRoom();
        }
    }

    private void OpenRoom()
    {
        // Disable the collider and tilemap
        if (disableTilemapCollider != null)
        {
            disableTilemapCollider.DisableTMCollider();
        }

        if (tilemapManager != null)
        {
            tilemapManager.DisableTilemap();
        }
    }

    private void CloseRoom()
    {
        // Disable the collider and tilemap
        if (disableTilemapCollider != null)
        {
            disableTilemapCollider.EnableTMCollider();
        }

        if (tilemapManager != null)
        {
            tilemapManager.EnableTilemap();
        }
    }
}
