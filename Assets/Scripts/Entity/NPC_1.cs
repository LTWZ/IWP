using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_1 : MonoBehaviour
{
    public DialogueTrigger trigger;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true && Input.GetKeyDown(KeyCode.F))
        {
            trigger.StartDialogue();
        }
    }
}
