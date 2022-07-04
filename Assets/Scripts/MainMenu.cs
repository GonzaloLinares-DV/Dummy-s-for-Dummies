using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public GameObject panel;
    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void TestScene()
    {
        SceneManager.LoadScene("Test");
    }

    public void TutorialScene()
    {
        SceneManager.LoadScene("TUTO");
    }

    public void Level2Scene()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Level3Scene()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void Level4Scene()
    {
        SceneManager.LoadScene("Level 4");
    }

    public void Level5Scene()
    {
        SceneManager.LoadScene("Level 5");
    }

    public void CreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelsPanelOpen()
    {
        panel.SetActive(true);
        transition.SetBool("Start", true);
    }

    public void LevelsPanelClose()
    {
        transition.SetBool("Start", false);

    }
    public void HidePanel()
    {
        panel.SetActive(false);
    }
}

