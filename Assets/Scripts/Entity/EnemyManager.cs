using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager GetInstance()
    {
        return instance;
    }

    private GameObject playerReference;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayerManager.GetInstance().onPlayerChange += ChangePlayerReference;
    }

    void ChangePlayerReference()
    {
        playerReference = PlayerManager.GetInstance().GetCurrentPlayer();
    }

    /// <summary>
    /// Get the player reference for any enemy that spawn to refer to
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayerReference()
    {
        return playerReference;
    }
}
