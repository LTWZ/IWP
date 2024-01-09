using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBoss : MonoBehaviour
{
    private bool playerInRange = false;
    [SerializeField] GameObject loadpanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerInRange)
        {
            LevelManager.GetInstance().LoadBossLevel();
            StartCoroutine(LoadLevelWithLoadingPanel());
        }
    }

    IEnumerator LoadLevelWithLoadingPanel()
    {
        // Instantiate the loading panel
        GameObject loadingPanelInstance = Instantiate(loadpanel);

        UIManager.GetInstance().HideAllUIElements();

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        UIManager.GetInstance().ShowAllUIElements();

        // Destroy the loading panel after loading the scene
        Destroy(loadingPanelInstance);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
