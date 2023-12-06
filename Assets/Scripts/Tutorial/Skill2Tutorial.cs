using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Tutorial : MonoBehaviour
{
    public Test1 Player;
    public bool canUseSkill2 = false;

    public static Skill2Tutorial instance;

    public static Skill2Tutorial GetInstance()
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
            player.canUseskill2 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerManager playerManager = PlayerManager.GetInstance();
            PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
            player.canUseskill2 = false;
        }
    }

}
