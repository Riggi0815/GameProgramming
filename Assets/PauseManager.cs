using UnityEngine;

public class PauseManager : MonoBehaviour {
    
    public static bool gameIsPaused = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                ResumeGame();
            }else
                GamePause();
        }
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    
    public void GamePause() {
        Time.timeScale = 0;
        gameIsPaused = true;
    }
    
}
