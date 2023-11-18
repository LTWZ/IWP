using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddManaTutorial : MonoBehaviour
{
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
            PlayerMovement.GetInstance().Player.ChangeMana(100);
        }
    }
}
