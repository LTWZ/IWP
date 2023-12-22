using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    public GameObject panelToClose;

    // Attach this method to the Button's OnClick event in the Unity Inspector
    public void ClosePanel()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel to close is not assigned!");
        }
    }
}
