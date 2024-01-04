using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        // load the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // quit the game
    public void QuitGame()
    {
        Debug.Log("Quit!");
        // quit game
        Application.Quit();
    }

    public void RestartGame()
    {
        // load the game
        SceneManager.LoadScene("SelectScreen");
    }
}
