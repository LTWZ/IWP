using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTutorialDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                dialogueManager.StartConversationInArea(messages, actors);
            }

            // Optional: Disable the trigger to prevent re-triggering.
            gameObject.SetActive(false);
        }
    }
}
