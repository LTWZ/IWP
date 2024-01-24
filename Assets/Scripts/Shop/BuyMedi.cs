using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyMedi : MonoBehaviour
{
    public TextMeshProUGUI debugLogText; // Reference to your TextMeshProUGUI component
    public GameObject door;

    public void BuyAK47()
    {
        PlayerEntity currentPlayer = PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>();

        if (currentPlayer.GetCurrCoins() >= 65)
        {
            door.SetActive(false);
            currentPlayer.ChangeCoins(-65);

            // Log a message to the TextMeshProUGUI
            if (debugLogText != null)
            {
                debugLogText.text = "Medi Gun bought!";
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
