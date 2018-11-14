using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Spawner spawner;
    public GameObject mainMenu;
    public GameObject winMenu;
    public GameObject LoseMenu;

    public enum MenuStates { Main, Lose, Won };
    public static MenuStates currentstate;

    void Awake()
    {
        currentstate = MenuStates.Main;
    }

    private void Update()
    {
        switch (currentstate)
        {
            case MenuStates.Main:
                mainMenu.SetActive(true);
                LoseMenu.SetActive(false);
                winMenu.SetActive(false);
                break;
            case MenuStates.Lose:
                LoseMenu.SetActive(true);
                mainMenu.SetActive(false);
                winMenu.SetActive(false);
                break;
            case MenuStates.Won:
                winMenu.SetActive(true);
                LoseMenu.SetActive(false);
                mainMenu.SetActive(false);
                break;
        }
    }

    public void OnStartGame()
    {
        spawner.enabled = true;
    }

    public void OnWinGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnTryAgain()
    {
        SceneManager.LoadScene("Level 1");
    }
}
