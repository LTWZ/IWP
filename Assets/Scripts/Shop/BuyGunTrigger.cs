using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGunTrigger : MonoBehaviour
{
    public GameObject panelToOpen;
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
            OpenPanel();
        }
    }

    private void OpenPanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel to open is not assigned!");
        }
    }
}
