using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// controller for the in-game GUI
public class GUI : MonoBehaviour
{
    [Tooltip("Score digit sprites from 0 to 9 in ascending order")]
    public List<Sprite> ScoreDigitSprites;

    // heart UI elements
    private List<GameObject> hearts;

    // score digit UI elements
    private List<GameObject> scoreDigitUIElements;

    // add a heart
    public void AddHeart()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            var heart = hearts[i];      
            if (heart.GetComponent<Image>().enabled == false)
            {
                heart.GetComponent<Image>().enabled = true;
                break;
            }
        }
    }

    // remove one of the hearts
    public void RemoveHeart()
    {
        for (int i = hearts.Count - 1; i >= 0; i--)
        {
            var heart = hearts[i];
            if (heart.GetComponent<Image>().enabled == true)
            {
                // Destroy(heart);
                heart.GetComponent<Image>().enabled = false;
                break;
            }
        }
    }

    // set score
    public void SetScore(int score)
    {
        // assert
        if (score < 0 || score > 9999)
        {
            throw new ArgumentException();
        }

        // sweep
        foreach (var number in scoreDigitUIElements)
        {
            number.GetComponent<Image>().enabled = false;
        }

        // set digits
        int numDigits = MathHelper.GetNumDigits(score);
        for (int i = 0; i < numDigits; i++)
        {
            GameObject number = scoreDigitUIElements[i];
            Image image = number.GetComponent<Image>();

            image.enabled = true;
            int digit = MathHelper.GetDigit(score, i + 1);
            image.sprite = ScoreDigitSprites[digit];
        }
    }

    private void Awake()
    {
        // collect objects
        hearts = GameObject.FindGameObjectsWithTag("Heart").OrderBy(x => x.transform.position.x).ToList();
        scoreDigitUIElements = GameObject.FindGameObjectsWithTag("ScoreDigit").OrderBy(x => x.transform.position.x).ToList();

        // set score
        SetScore(0);
    }
}