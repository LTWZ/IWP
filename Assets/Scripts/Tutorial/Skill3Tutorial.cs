using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3Tutorial : MonoBehaviour
{
    public Test1 Player;
    public bool canUseSkill3 = false;

    public static Skill3Tutorial instance;

    public static Skill3Tutorial GetInstance()
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
            PlayerMovement.GetInstance().Player.canUseskill3 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement.GetInstance().Player.canUseskill3 = false;
        }
    }

}
