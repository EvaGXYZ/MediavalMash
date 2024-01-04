using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    // setup boolean to check is game paused
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    //public GameObject largeFont;


    void Update()
    {
        //check if players pause the game by press Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // opposite of method Pause();
    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        // set time back to normal
        Time.timeScale = 1f;

        GameIsPaused = false;

    }

    // active pause menu, freeze the game
    public void Pause()
    {
        // active pause menu
        pauseMenuUI.SetActive(true);

        // freeze the game
        Time.timeScale = 0f;

        // update the boolean
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        // set time back to normal
        Time.timeScale = 1f;

        // load menu scene
        SceneManager.LoadScene("Title");
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    /*public void LargeFont()
    {
        //change to large font dialogue box
        largeFont.SetActive(true);
    }*/

    public void QuitGame()
    {
        Application.Quit();
    }

}

