using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_1 : MonoBehaviour
{
    public DialogueTrigger trigger;
    private bool inTriggerZone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTriggerZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTriggerZone = false;
        }
    }

    private void Update()
    {
        if (inTriggerZone && Input.GetKeyDown(KeyCode.F))
        {
            trigger.StartDialogue();
        }
    }
}

