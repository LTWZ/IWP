using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    private void Start()
    {
        // Attach the button click event
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SceneChange);
    }

    private void SceneChange()
    {
        // Load the target scene
        SceneManager.LoadScene(targetSceneName);

    }
}
