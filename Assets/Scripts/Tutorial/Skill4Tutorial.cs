using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4Tutorial : MonoBehaviour
{
    public Test1 Player;
    public bool canUseSkill4 = false;

    public static Skill4Tutorial instance;

    public static Skill4Tutorial GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerManager playerManager = PlayerManager.GetInstance();
            PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
            player.canUseskill4 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerManager playerManager = PlayerManager.GetInstance();
            PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
            player.canUseskill4 = false;
        }
    }
}