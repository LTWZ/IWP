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
    private bool isWarriorTutorialComplete;

    private void Start()
    {
        selectWizardButton.onClick.AddListener(delegate { SelectClass(PlayerType.WIZARD); });
        selectRogueButton.onClick.AddListener(delegate { SelectClass(PlayerType.ROGUE); });
        selectWarriorButton.onClick.AddListener(delegate { SelectClass(PlayerType.WARRIOR); });
        pm = PlayerManager.GetInstance();

    }

    void SelectClass(PlayerType whichPlayerType)
    {
        isWizardTutorialComplete = PlayerPrefs.GetInt("WizTutComplete") == 1? true : false;
        isRogueTutorialComplete = PlayerPrefs.GetInt("RogTutComplete") == 1? true : false;
        isWarriorTutorialComplete = PlayerPrefs.GetInt("WarTutComplete") == 1 ? true : false;
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
            AudioManager.instance.PlaySFX("Button Click");

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
            AudioManager.instance.PlaySFX("Button Click");


        }
        if (whichPlayerType == PlayerType.WARRIOR)
        {
            if (isWarriorTutorialComplete == false)
            {
                SceneManager.LoadScene("FighterTutorial");
            }
            else if (isRogueTutorialComplete == true)
            {
                SceneManager.LoadScene("TutorialLevel");
            }
            AudioManager.instance.PlaySFX("Button Click");
        }
        /*SceneManager.LoadScene("TutorialLevel");*/
    }
}
