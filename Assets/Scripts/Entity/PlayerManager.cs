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
    private bool canLoadData = false;
    private int currHealth = 0;
    private int currMana = 0;

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

        if (canLoadData)
        {
            playerCreated.GetComponent<PlayerEntity>().SetCurrHealth(currHealth);
            playerCreated.GetComponent<PlayerEntity>().SetCurrMana(currMana);
        }
        else
        {
            canLoadData = true;
        }

        onPlayerChange?.Invoke();
    }

    public GameObject GetCurrentPlayer()
    {
        return playerCreated;
    }

    public void SavePlayerData()
    {
        currHealth = playerCreated.GetComponent<PlayerEntity>().GetCurrHealth();
        currMana = playerCreated.GetComponent<PlayerEntity>().GetCurrMana();
    }
}