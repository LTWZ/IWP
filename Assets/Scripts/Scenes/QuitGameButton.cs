using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuitGameButton : MonoBehaviour
{

    public void QuitGame()
    {
        // Quit the application
        Debug.Log("Quit game!");
        Application.Quit();
    }
}