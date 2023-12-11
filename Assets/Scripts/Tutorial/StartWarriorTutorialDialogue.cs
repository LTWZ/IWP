using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWarriorTutorialDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    [SerializeField] GameObject door;
    private bool isDialogueFinished = false;
    private bool isStartTutorialComplete = false;

    public void Start()
    {
        // Load the completion status from PlayerPrefs
        isStartTutorialComplete = PlayerPrefs.GetInt("StartTutComplete") == 1 ? true : false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                // Check if the gun tutorial is not complete
                if (!isStartTutorialComplete)
                {
                    // Start the conversation
                    dialogueManager.StartConversationInArea(messages, actors);

                    // Set flags and save completion status in PlayerPrefs
                    isDialogueFinished = true;
                    isStartTutorialComplete = true;
                    PlayerPrefs.SetInt("GunTutComplete", isStartTutorialComplete ? 1 : 0);
                }
            }

            // Optional: Disable the trigger to prevent re-triggering.
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the dialogue is finished and the tutorial is complete
        if (isDialogueFinished && isStartTutorialComplete)
        {
            // Disable the door
            door.SetActive(false);
        }
    }
}