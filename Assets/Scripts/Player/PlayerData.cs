using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Add fields for persistent player data (e.g., mana)
    private int mana;

    public void SetMana(int value)
    {
        mana = value;
    }

    public int GetMana()
    {
        return mana;
    }
}
