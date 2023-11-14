using UnityEngine;
using UnityEngine.Tilemaps;

public class DisableTilemapCollider : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    private static DisableTilemapCollider instance;
    public static DisableTilemapCollider GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    DisableTMCollider();
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    EnableTMCollider();
        //}
    }

    // Example method to disable the Tilemap Collider
    public void DisableTMCollider()
    {
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = false;
        }
    }

    // Example method to enable the Tilemap Collider
    public void EnableTMCollider()
    {
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = true;
        }
    }
}
