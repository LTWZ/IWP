using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyHealthPotion : MonoBehaviour
{
    public TextMeshProUGUI debugLogText; // Reference to your TextMeshProUGUI component

    public void BuyHealthPotions()
    {
        PlayerEntity currentPlayer = PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>();

        if (currentPlayer.GetCurrCoins() >= 20)
        {
            currentPlayer.ChangeHealthPotionAmt(1);
            currentPlayer.ChangeCoins(-20);

            // Log a message to the TextMeshProUGUI
            if (debugLogText != null)
            {
                debugLogText.text = "Health potion bought!";
            }
        }
        else
        {
            // Log a message to the TextMeshProUGUI
            if (debugLogText != null)
            {
                debugLogText.text = "Not enough coins!";
            }
        }
    }
}
