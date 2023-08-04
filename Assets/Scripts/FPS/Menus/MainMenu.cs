using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    //Start Game
    public void PlayGame() {

        SceneManager.LoadScene("Playscene");

    }

    public void ShowHighscores() {

        SceneManager.LoadScene("HighscoreScene");

    }
    
}
