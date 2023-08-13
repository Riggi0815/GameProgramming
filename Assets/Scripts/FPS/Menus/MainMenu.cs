using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static bool wasPlayed = false;
    
    //Start Game
    public void PlayGame() {

        wasPlayed = true;
        SceneManager.LoadScene("Playscene");

    }

    //Load Highscore Scene
    public void ShowHighscores() {

        SceneManager.LoadScene("HighscoreScene");

    }

    //End Game
    public void EndGame() {
        Debug.Log("End App!");
        //Only working in Build App
        Application.Quit();
    }
    
}
