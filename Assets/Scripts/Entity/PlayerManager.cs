using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager GetInstance()
    {
        return instance;
    }

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


    private int playerHp;
    private int playerMana;

    public void UpdateHP(int hp)
    {
        playerHp = hp;
    }

    public int LoadHP()
    {
        return playerHp;
    }

    public void UpdateMana(int mana)
    {
        playerMana = mana;
    }

    public int LoadMana()
    {
        return playerMana;
    }
}
