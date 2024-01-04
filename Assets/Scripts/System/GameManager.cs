using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //public List<PlayerController> players = new List<PlayerController>();

    // for loading scene
    private int currentSceneNumber;
    [SerializeField]
    private int waitTime = 5;
    private int resultSceneIndex = 7;
    private int forestSceneIndex = 3;
    private int desertSceneIndex = 4;
    private int winterSceneIndex = 5;
    private bool isLoading = false;
    // checking players status and scores
    private int deadPlayers = 0;
    private int deadPlayerIndex;
    private List<int> deathOrders = new List<int>();
    private string[] playerScoreString = {"scoreOfPlayer1", "scoreOfPlayer2", "scoreOfPlayer3", "scoreOfPlayer4", };
    // system save
    public PlayerController[] players;
    // score display
    public ScoreBoard[] scoreBoards;

    // clean weapons and tools when load new scene
    private Weapon[] weapons;
    private Tool[] tools;

    private float awakeTime;

    private void Awake()
    {
        // keep the same game manager through the game play
        DontDestroyOnLoad(this.gameObject);
        // get players
        players = FindObjectsOfType<PlayerController>();

        // keep players info through scenes
        for (int i = 0; i < players.Length; i++)
        {
            //print(i +" player in array is player "+ players[i].GetPlayerIndex());
            DontDestroyOnLoad(players[i].gameObject);
        }
        
        // reset score
        ResetScore();
        PrintScore();
    }

    // run everytime when load a new scene
    private void OnLevelWasLoaded(int level)
    {
        // destory all the weapons
        weapons = FindObjectsOfType<Weapon>();
        // keep players info through scenes
        for (int i = 0; i < weapons.Length; i++)
        {
            //print(i +" player in array is player "+ players[i].GetPlayerIndex());
            Destroy(weapons[i].gameObject);
        }
        // destory all the weapons
        tools = FindObjectsOfType<Tool>();
        // keep players info through scenes
        for (int i = 0; i < tools.Length; i++)
        {
            //print(i +" player in array is player "+ players[i].GetPlayerIndex());
            Destroy(tools[i].gameObject);
        }

        //print("<color=cyan>In on level was loaded, level Number is </color>" + level);
        // reset the deadplayer number and deathlist
        ResetDeathList();
        // reset bool to load next scene
        isLoading = false;

        // if load result scene
        if (level == resultSceneIndex)
        {
            // move players away from Canvas
            OffScreen();
            //load score
            DisplayPlayerScores();
            // start load next scene
            StartCoroutine(LoadNextScene());
            
        }
        else if (level == desertSceneIndex || level == winterSceneIndex || level == forestSceneIndex || level == 6)
        {
            // update the scene number
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("currentSceneNumber", currentSceneNumber);
            if (players != null)
            {
                // put players back and load score
                BacKOnScreen();
                ReactivePlayers();
                for (int i = 0; i < players.Length; i++)
                {
                    //print(i + " player in array is player " + players[i].GetPlayerIndex());
                    //players[i].playerScore = LoadScore(playerScoreString[i]);
                }
                GetInitialPoints();
            }
        }// keep only one game manager and load in the new game
        else if (level == 0)
        {
            //game manager is destroyable
            //can carry the game manager for next scene
            Destroy(this.gameObject);

            // keep players info through scenes
            for (int i = 0; i < players.Length; i++)
            {
                //print(i +" player in array is player "+ players[i].GetPlayerIndex());
                Destroy(players[i].gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (players!=null)
        {
            // check whether player is dead or not
            PlayerCurrentStatus();
        }

        // if in the game scene and only one player alive
        if (SceneManager.GetActiveScene().buildIndex < resultSceneIndex && deadPlayers >= players.Length - 1)
        {
            CalculateScore();
            //PrintScore();
            //SaveScores();
            // reset death list and dead players number
            ResetDeathList();
            // load result scene
            StartCoroutine(WaitOneSecond());
            DeactivePlayers();
            SceneManager.LoadScene("Result");
        }
    }

    // wait for 5 second to load next scene
    public IEnumerator LoadNextScene()
    {
        // wait for 5s
        yield return new WaitForSeconds(waitTime);
        // load next game scene
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber")+1);
    }

    public IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1f);
        isLoading = true;
    }

    // Calculate Score
    public void CalculateScore()
    {
        // minus the score players lost by the death order
        players[deathOrders[0]].playerScore -= 3;
        players[deathOrders[1]].playerScore -= 2;
        players[deathOrders[2]].playerScore -= 1;
    }

    public void PrintScore()
    {
        print("Player 1 current score: " + players[0].playerScore);
        print("Player 2 current score: " + players[1].playerScore);
        print("Player 3 current score: " + players[2].playerScore);
        print("Player 4 current score: " + players[3].playerScore);
    }
    // save current score
    public void SaveScores()
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerPrefs.SetInt(playerScoreString[i], players[i].playerScore);
        }
    }
    // load player's score
    public int LoadScore(string playerScore)
    {
         return PlayerPrefs.GetInt(playerScore);
    }
    // display all players' score on Canvas
    public void DisplayPlayerScores()
    {
        // get score board
        ScoreBoard[] scoreBoards = FindObjectsOfType<ScoreBoard>();
        //load score
        for (int i = 0; i < players.Length; i++)
        {
            //players[i].playerScore = LoadScore(playerScoreString[i]);
            scoreBoards[i].DisplayPlayerScore(players[i]);
        }
    }
    // reset score to 0
    public void ResetScore()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerScore = 0;
        }
    }

    // add three points to each players
    public void GetInitialPoints()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerScore += 3;
        }
    }

    // check if player is dead
    public void PlayerCurrentStatus()
    {
        for (int i = 0; i < players.Length; i++)
        {
            // when player is dead
            if (players[i].playerIsDead)
            {
                // get dead player's index
                deadPlayerIndex = players[i].GetPlayerIndex();
                // if player is not in the dead list
                if (!deathOrders.Contains(deadPlayerIndex))
                {
                    // update dead players number
                    deadPlayers++;
                    // add this player into the list
                    deathOrders.Add(deadPlayerIndex);
                    print("Player " + (players[i].GetPlayerIndex() + 1) + " is dead.");
                }
            }
        }
    }

    // reactive players
    public void ReactivePlayers()
    {
        if (players != null)
        {
            // reactive players
            for (int i = 0; i < players.Length; i++)
            {
                players[i].gameObject.SetActive(true);
            }
        }
    }

    public void DeactivePlayers()
    {
        if (players != null)
        {
            // reactive players
            for (int i = 0; i < players.Length; i++)
            {
                players[i].gameObject.SetActive(false);
            }
        }
    }

    // send players off screen
    public void OffScreen()
    {
        if (players != null)
        {
            // send players off screen
            for (int i = 0; i < players.Length; i++)
            {
                players[i].gameObject.transform.position = new Vector3(-18.6f, 9.14f, 0);
            }
        }
    }
    // put players back on screen
    public void BacKOnScreen()
    {
        if (players != null)
        {
            // send players off screen
            players[0].gameObject.transform.position = new Vector3(-3.59f, 1.86f, 0);
            players[1].gameObject.transform.position = new Vector3(-3.59f, -1.09f, 0);
            players[2].gameObject.transform.position = new Vector3(4.43f, 1.86f, 0);
            players[3].gameObject.transform.position = new Vector3(4.43f, -1.09f, 0);

        }
    }
    // reset game
    public void ResetDeathList()
    {
        // reset the deadplayer number and deathlist
        deadPlayers = 0;
        deathOrders = new List<int>();
    }
}
