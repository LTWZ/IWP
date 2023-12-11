using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTutorialDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    [SerializeField] GameObject door;
    private bool isDialogueFinished = false;
    private bool isGunTutorialComplete = false;

    public void Start()
    {
        // Load the completion status from PlayerPrefs
        isGunTutorialComplete = PlayerPrefs.GetInt("GunTutComplete") == 1 ? true : false;
        if (isGunTutorialComplete)
        {
            // Disable the door
            door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                // Check if the gun tutorial is not complete
                if (!isGunTutorialComplete)
                {
                    // Start the conversation
                    dialogueManager.StartConversationInArea(messages, actors);

                    // Set flags and save completion status in PlayerPrefs
                    isDialogueFinished = true;
                    isGunTutorialComplete = true;
                    PlayerPrefs.SetInt("GunTutComplete", isGunTutorialComplete ? 1 : 0);
                }
            }

            // Optional: Disable the trigger to prevent re-triggering.
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the dialogue is finished and the tutorial is complete
        if (isDialogueFinished)
        {
            // Disable the door
            door.SetActive(false);
        }
    }
}