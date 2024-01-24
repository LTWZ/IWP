using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyManaPotion : MonoBehaviour
{
    public TextMeshProUGUI debugLogText; // Reference to your TextMeshProUGUI component

    public void BuyManaPotions()
    {
        PlayerEntity currentPlayer = PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>();

        if (currentPlayer.GetCurrCoins() >= 20)
        {
            currentPlayer.ChangeManaPotionAmt(1);
            currentPlayer.ChangeCoins(-20);

            // Log a message to the TextMeshProUGUI
            if (debugLogText != null)
            {
                debugLogText.text = "Mana potion bought!";
                AudioManager.instance.PlaySFX("CanBuy");
            }
        }
        else
        {
            // Log a message to the TextMeshProUGUI
            if (debugLogText != null)
            {
                debugLogText.text = "Not enough coins!";
                AudioManager.instance.PlaySFX("NoBuy");
            }
        }
    }
}
