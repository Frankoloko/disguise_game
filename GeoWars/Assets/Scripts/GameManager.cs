using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    static int starsInScene;
    public static int collectedStars = 0;
    public static Text scoreText;

    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();

        // Get the total stars in scene
        starsInScene = GameObject.FindGameObjectsWithTag("Star").Length;

        // Set the starting score text
        scoreText.text = collectedStars.ToString() + " / " + starsInScene.ToString();
    }

    public static void IncreaseCollectedStars()
    {
        // Increase collectedStars by one & update the score label on screen
        collectedStars += 1;
        scoreText.text = "Rings collected: " + collectedStars.ToString() + " / " + starsInScene.ToString();
        Debug.Log(scoreText.text);
    }
}
