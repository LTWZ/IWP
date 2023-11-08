using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public GameObject warriorPrefab; // Assign your player prefabs in the Inspector.
    public GameObject wizardPrefab;
    public GameObject roguePrefab;

    public void ChooseWarrior()
    {
        PlayerPrefs.SetString("SelectedClass", "Warrior");
        LoadNextScene();
    }

    public void ChooseMage()
    {
        PlayerPrefs.SetString("SelectedClass", "Wizard");
        LoadNextScene();
    }

    public void ChooseRogue()
    {
        PlayerPrefs.SetString("SelectedClass", "Rogue");
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("TutorialLevel"); // Load the next scene (change "Gameplay" to your scene name).
    }
}
