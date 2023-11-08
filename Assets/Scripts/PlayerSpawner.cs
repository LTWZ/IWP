using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject warriorPrefab; // Assign your player prefabs in the Inspector.
    public GameObject wizardPrefab;
    public GameObject roguePrefab;

    void Start()
    {
        string selectedClass = PlayerPrefs.GetString("SelectedClass");

        if (selectedClass == "Warrior")
        {
            Instantiate(warriorPrefab, transform.position, Quaternion.identity);
        }
        else if (selectedClass == "Mage")
        {
            Instantiate(wizardPrefab, transform.position, Quaternion.identity);
        }
        else if (selectedClass == "Rogue")
        {
            Instantiate(roguePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No selected class found. Defaulting to a specific class.");
            // Instantiate a default class or show an error message.
        }
    }
}
