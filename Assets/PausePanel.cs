using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public void ReturnToGame()
    {
        AudioManager.instance.PlaySFX("Button Click");
        Time.timeScale = 1;
        LevelManager.GetInstance().isPausePanelInstantiated = false;
        Destroy(gameObject);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlaySFX("Button Click");
        SceneManager.LoadScene("StartMenu");
        Destroy(PlayerManager.GetInstance().gameObject);
        Destroy(EnemyManager.GetInstance().gameObject);
        Destroy(LevelManager.GetInstance().gameObject);
        Destroy(WeaponManager.GetInstance().gameObject);
    }
}
