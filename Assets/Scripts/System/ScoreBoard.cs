using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public int currentScore;
    private int maxScores = 9;

    public Image[] scores;

    private void OnLevelWasLoaded(int level)
    {
        if (level <= 3)
        {
            currentScore = 0;
        }
    }
    public void DisplayPlayerScore(PlayerController player)
    {
        // get score
        currentScore = player.playerScore;

        // make sure it is not over the max
        if (currentScore > maxScores)
        {
            currentScore = maxScores;
        }
        // enable or disable the crown
        for (int j = 0; j < scores.Length; j++)
        {

            if (j < currentScore)
            {
                scores[j].enabled = true;
            }
            else
            {
                scores[j].enabled = false;
            }
        }
    }
}
