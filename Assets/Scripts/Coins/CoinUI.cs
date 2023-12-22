//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class CoinUI : MonoBehaviour
//{
//    public static CoinUI Instance;
//    public TextMeshProUGUI coinText;
//    public int coinCount;

//    void Start()
//    {
//        Instance = this;
//        coinCount = 0;
//        coinText.text = " " + PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrCoins


//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.P)) // Change KeyCode.Space to the key you want to use
//        {
//            coinCount += 10;
//            coinText.text = " " + coinCount;
//        }
//    }
//}
