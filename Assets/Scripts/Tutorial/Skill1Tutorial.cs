using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Tutorial : MonoBehaviour
{
    public Test1 Player;
    public bool canUseSkill1 = false;
    public static Skill1Tutorial instance;
    public DialogueTrigger trigger;
    private bool inTriggerZone = false;

    public static Skill1Tutorial GetInstance()
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
            player.canUseskill1 = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerManager playerManager = PlayerManager.GetInstance();
            PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
            player.canUseskill1 = false;
        }
    }


}
