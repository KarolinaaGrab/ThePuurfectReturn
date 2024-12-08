using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsPanel;

    public void CreditsPanelActivate()
    {
        mainMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CreditsPanelDeactivate()
    {
        creditsPanel.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
