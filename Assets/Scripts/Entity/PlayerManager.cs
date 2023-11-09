using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum PlayerType
{
    WARRIOR,
    WIZARD,
    ROGUE,
}

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager GetInstance()
    {
        return instance;
    }

    [SerializeField] GameObject wizardPrefab;
    [SerializeField] GameObject roguePrefab;
    [SerializeField] GameObject warriorPrefab;
    private PlayerType chosenPlayerType;
    private GameObject playerCreated;

    public delegate void OnPlayerChange();
    public OnPlayerChange onPlayerChange;

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

    /// <summary>
    /// Set the player type that the player will run for the whole game
    /// </summary>
    public void SetPlayerType(PlayerType playerType)
    {
        chosenPlayerType = playerType;
    }

    /// <summary>
    /// Create the player according to what playertype is chosen.
    /// </summary>
    public void CreatePlayer()
    {
        // remove the previous gameobject reference
        playerCreated = null;
        switch (chosenPlayerType)
        {
            case PlayerType.WIZARD:
                playerCreated = Instantiate(wizardPrefab, transform.position, Quaternion.identity);
                break;
            case PlayerType.ROGUE:
                playerCreated = Instantiate(roguePrefab, transform.position, Quaternion.identity);
                break;
            case PlayerType.WARRIOR:
                playerCreated = Instantiate(warriorPrefab, transform.position, Quaternion.identity);
                break;
        }
        GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineVirtualCamera>().Follow = playerCreated.transform;
        onPlayerChange?.Invoke();
    }

    public GameObject GetCurrentPlayer()
    {
        return playerCreated;
    }
}
