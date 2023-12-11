using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTutorialDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public bool isWizTutorialComplete = false;
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
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isWizTutorialComplete = true;
             PlayerPrefs.SetInt("WizTutComplete", (isWizTutorialComplete ? 1 : 0));
            // Optional: Disable the trigger to prevent re-triggering.
            gameObject.SetActive(false);
        }
    }
}
