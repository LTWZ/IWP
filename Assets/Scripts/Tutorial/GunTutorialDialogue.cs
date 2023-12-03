using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTutorialDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    [SerializeField] GameObject door;
    private bool isDialogueFinished = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                dialogueManager.StartConversationInArea(messages, actors);
                isDialogueFinished = false;
            }

            // Optional: Disable the trigger to prevent re-triggering.
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDialogueFinished == false)
        {
            door.SetActive(false);
        }
    }
}
