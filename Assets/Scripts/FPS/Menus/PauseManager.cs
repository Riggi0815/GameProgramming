using UnityEngine;

public class PauseManager : MonoBehaviour {
    
    public static bool gameIsPaused = false;

    public GameObject pauseOverlay;
    public GameObject optionsOverlay;

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

    public void ResumeGame() {
        
        Time.timeScale = 1;
        gameIsPaused = false;
        pauseOverlay.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void GamePause() {
        Time.timeScale = 0;
        gameIsPaused = true;
        pauseOverlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void PauseMenu() {
        pauseOverlay.SetActive(true);
        optionsOverlay.SetActive(false);
    }
    
}
