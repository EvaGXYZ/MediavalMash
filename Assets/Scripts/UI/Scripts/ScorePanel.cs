using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    private Sprite[] digits;

    [SerializeField]
    private Image[] characters;

    private int scoreAmount;
    private int numberOfDigits;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 1; i++)
        {
            characters[i].sprite = digits[0];
        }

        scoreAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    public int SetScoreAmount(int score)
    {
        scoreAmount = score;
        return scoreAmount;
    }

    private void DisplayScore()
    {
        int[] scoreAmountByDigits = GetDigitsArray(scoreAmount);

        switch (scoreAmountByDigits.Length)
        {
            case 1:
                characters[0].sprite = digits[0];
                characters[1].sprite = digits[scoreAmountByDigits[0]];
                break;
            case 2:
                characters[0].sprite = digits[scoreAmountByDigits[0]];
                characters[1].sprite = digits[scoreAmountByDigits[1]];
                break;
        }
    }

    private int[] GetDigitsArray(int scoreAmount)
    {
        List<int> listOfInts = new List<int>();

        while (scoreAmount > 0)
        {
            listOfInts.Add(scoreAmount % 10);
            scoreAmount = scoreAmount / 10;
        }

        listOfInts.Reverse();
        return listOfInts.ToArray();
    }
}
