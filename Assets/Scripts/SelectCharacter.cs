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
    private bool isWizardTutorialComplete;
    private bool isRogueTutorialComplete;

    private void Start()
    {
        selectWizardButton.onClick.AddListener(delegate { SelectClass(PlayerType.WIZARD); });
        selectRogueButton.onClick.AddListener(delegate { SelectClass(PlayerType.ROGUE); });
        //selectWarriorButton.onClick.AddListener(delegate { SelectClass(PlayerType.WARRIOR); });
        pm = PlayerManager.GetInstance();

    }

    void SelectClass(PlayerType whichPlayerType)
    {
        isWizardTutorialComplete = PlayerPrefs.GetInt("WizTutComplete") == 1? true : false;
        isRogueTutorialComplete = PlayerPrefs.GetInt("RogTutComplete") == 1? true : false;
        PlayerManager.GetInstance().SetPlayerType(whichPlayerType);
        if (whichPlayerType == PlayerType.WIZARD)
        {
            if (isWizardTutorialComplete == false)
            {
                SceneManager.LoadScene("WizardTutorial");
            }
            else if (isWizardTutorialComplete == true)
            {
                SceneManager.LoadScene("TutorialLevel");
            }
            
        }
        if (whichPlayerType == PlayerType.ROGUE)
        {
            if (isRogueTutorialComplete == false)
            {
                SceneManager.LoadScene("RogueTutorial");
            }
            else if (isRogueTutorialComplete == true)
            {
                SceneManager.LoadScene("TutorialLevel");
            }

        }
        if (whichPlayerType == PlayerType.WARRIOR)
        {
            SceneManager.LoadScene("WarriorTutorial");
        }
        /*SceneManager.LoadScene("TutorialLevel");*/ // Load the next scene (change "Gameplay" to your scene name).
    }
}
