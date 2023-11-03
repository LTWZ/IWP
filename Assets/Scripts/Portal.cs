using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerInRange)
        {
            LoadRandomLevel();
        }
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

    private void LoadRandomLevel()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.availableLevels.Count > 0)
        {
            int randomIndex = Random.Range(0, gameManager.availableLevels.Count);
            string randomLevelName = gameManager.availableLevels[randomIndex];
            gameManager.availableLevels.RemoveAt(randomIndex);
            SceneManager.LoadScene(randomLevelName);
        }
        else
        {
            Debug.Log("No more available levels.");
        }
    }
}