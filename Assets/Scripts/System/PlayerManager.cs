using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<CharManager> playerSprite;
    [SerializeField]
    private int MaxPlayers = 2;
    bool isLoading = false;

    private void Update()
    {
        if(!isLoading)
        {
            StartCoroutine(WaitOneSecond());
            ReadyPlayer(MaxPlayers);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ReadyPlayer(int index)
    {
        // check if all players are ready
        if (playerSprite.Count == MaxPlayers && playerSprite.All(p => p.playerIsReady == true))
        {
            //StartCoroutine(WaitOneSecond());
            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        isLoading = true;
    }
}
