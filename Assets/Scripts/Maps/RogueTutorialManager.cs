using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueTutorialManager : MonoBehaviour
{
    private BoxCollider2D currentBoxcollider;
    private void Start()
    {
        currentBoxcollider = GetComponent<BoxCollider2D>();
        OpenExit(); // Start with the exit open
    }

    // Called when the player enters the room
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Rogue"))
            {
                OpenExit();
            }
            else
            {

            }
        }
    }

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
