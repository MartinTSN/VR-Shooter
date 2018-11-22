using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Spawner spawner;
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject waveMenu;
    public GameObject winMenu;
    public GameObject LoseMenu;

    bool doOnce = false;

    public enum MenuStates { Main, Game, Wave, Lose, Won };
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
                gameMenu.SetActive(false);
                waveMenu.SetActive(false);
                break;
            case MenuStates.Lose:
                LoseMenu.SetActive(true);
                mainMenu.SetActive(false);
                winMenu.SetActive(false);
                gameMenu.SetActive(false);
                waveMenu.SetActive(false);
                break;
            case MenuStates.Won:
                winMenu.SetActive(true);
                OnWaveEnd();
                LoseMenu.SetActive(false);
                mainMenu.SetActive(false);
                gameMenu.SetActive(false);
                waveMenu.SetActive(false);
                break;
            case MenuStates.Game:
                gameMenu.SetActive(true);
                LoseMenu.SetActive(false);
                winMenu.SetActive(false);
                mainMenu.SetActive(false);
                waveMenu.SetActive(false);
                break;
            case MenuStates.Wave:
                waveMenu.SetActive(true);
                OnWaveEnd();
                LoseMenu.SetActive(false);
                winMenu.SetActive(false);
                mainMenu.SetActive(false);
                gameMenu.SetActive(false);
                break;
        }
    }

    public void OnWaveEnd()
    {
        if (doOnce == false)
        {
            GameObject Rhand = GameObject.Find("RightHand");
            Rhand.GetComponent<LineRenderer>().enabled = true;
            Rhand.GetComponent<Laser>().enabled = true;
            Rhand.GetComponent<MenuInteract>().enabled = true;
            GameObject Lhand = GameObject.Find("LeftHand");
            Lhand.GetComponent<LineRenderer>().enabled = true;
            Lhand.GetComponent<Laser>().enabled = true;
            Lhand.GetComponent<MenuInteract>().enabled = true;
            Rhand.GetComponentInChildren<Attach>().UnSet();
            doOnce = true;
        }
    }

    public void OnStartGame()
    {
        doOnce = false;
        spawner.enabled = true;
        currentstate = MenuStates.Game;
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
