using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public TilemapRenderer tilemapRenderer;
    private static TilemapManager instance;
    public static TilemapManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Disable the Tilemap initially
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    DisableTilemap(); 
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    EnableTilemap();
        //    Debug.Log("why");
        //}
    }

    // Example method to disable the Tilemap
    public void DisableTilemap()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false;
        }
    }

    // Example method to enable the Tilemap
    public void EnableTilemap()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = true;
        }
    }
}
