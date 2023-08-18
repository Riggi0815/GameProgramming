using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    
    public static bool gameIsPaused = false;

    public GameObject pauseOverlay;
    public GameObject optionsOverlay;

    //On ESC PAuse or Resume Game
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                if (optionsOverlay.activeSelf == true) {
                    PauseMenu();
                }
                else {
                    ResumeGame();
                }
            }else
                GamePause();
        }
    }

    //Starts the Game agarin
    public void ResumeGame() {
        
        Time.timeScale = 1;
        gameIsPaused = false;
        pauseOverlay.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    //Pauses the Game
    public void GamePause() {
        Time.timeScale = 0;
        gameIsPaused = true;
        pauseOverlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    //Opens Pause Menu
    public void PauseMenu() {
        pauseOverlay.SetActive(true);
        optionsOverlay.SetActive(false);
    }
    
    //Opens Main Menu again
    public void GoHome() {
        MainMenu.wasPlayed = false;
        SceneManager.LoadScene("Menuscene");
        Time.timeScale = 1;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
