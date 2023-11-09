using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] Button selectWizardButton;
    [SerializeField] Button selectRogueButton;
    [SerializeField] Button selectWarriorButton;
    PlayerManager pm;

    private void Start()
    {
        selectWizardButton.onClick.AddListener(delegate { SelectClass(PlayerType.WIZARD); });
        selectRogueButton.onClick.AddListener(delegate { SelectClass(PlayerType.ROGUE); });
        //selectWarriorButton.onClick.AddListener(delegate { SelectClass(PlayerType.WARRIOR); });
        pm = PlayerManager.GetInstance();
    }

    void SelectClass(PlayerType whichPlayerType)
    {
        PlayerManager.GetInstance().SetPlayerType(whichPlayerType);
        SceneManager.LoadScene("TutorialLevel"); // Load the next scene (change "Gameplay" to your scene name).
    }
}
