using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static int curScore;
    public Text scoreText;
    
    //adds a value to the UI Score
    public void AddScore(int points) {
        curScore += points;
        scoreText.text = curScore.ToString();
    }
    
}
