using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Tutorial : MonoBehaviour
{
    public Test1 Player;
    public bool canUseSkill1 = false;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement.GetInstance().Player.canUseskill1 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement.GetInstance().Player.canUseskill1 = false;
        }
    }
}
