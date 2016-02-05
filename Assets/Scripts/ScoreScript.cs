using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    public static int score;
    public Text scoreText;

    void Start()
    {
        scoreText.text = score.ToString();
    }

    public static void increment(int amount)
    {
        score += amount;        
        print("Score: " + score);
    }

    void Update()
    {
        scoreText.text = score.ToString();
    }
}
