using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour {

    private Transform highscoreContainer;
    private Transform highscoreTemplate;

    [SerializeField] private string filename;

    private List<Transform> highscoreEntriesTransforms;

    private void Start() {

        Cursor.lockState = CursorLockMode.None;
        
        //References to Transforms
        highscoreContainer = transform.Find("HighscoreContainer");
        highscoreTemplate = highscoreContainer.Find("HighscoreTemplate");
        
        highscoreTemplate.gameObject.SetActive(false);

        //If Game was Played then Create new Entry
        if (MainMenu.wasPlayed) {
            AddHighscoreEntry(GetScore());
            MainMenu.wasPlayed = false;
        }
        
        //Load the Highscores
        Highscores highscores = new Highscores();
        highscores.highscoreEntries = FileHandler.ReadFromJSON<HighscoreEntry>(filename);
        
        //Sorting the Scores
        for (int i = 0; i < highscores.highscoreEntries.Count; i++) {
            for (int j = i + 1; j < highscores.highscoreEntries.Count; j++) {
                if (highscores.highscoreEntries[j].score > highscores.highscoreEntries[i].score) {
                    //Switching
                    HighscoreEntry tmp = highscores.highscoreEntries[i];
                    highscores.highscoreEntries[i] = highscores.highscoreEntries[j];
                    highscores.highscoreEntries[j] = tmp;
                }
            }
        }
        
        //Display only 10 Scores
        if (highscores.highscoreEntries.Count > 10) {
            for (int h = highscores.highscoreEntries.Count; h > 10; h--) {
                highscores.highscoreEntries.RemoveAt(10);
            }
        }

        //Creates Transforms
        highscoreEntriesTransforms = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntries) {
            CreateHighscoreEntryTransform(highscoreEntry, highscoreContainer, highscoreEntriesTransforms);
        }
    }

    //Creates the Entrys
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 50f;
        
        Transform highscoreTransform = Instantiate(highscoreTemplate, container);
        RectTransform highscoreRectTransform = highscoreTransform.GetComponent<RectTransform>();
        highscoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        highscoreTransform.gameObject.SetActive(true);

        //Rank 1 - 10
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
            default:
                rankString = rank + "."; break;
        }
        highscoreTransform.Find("RankText").GetComponent<Text>().text = rankString;

        //Score 
        var score = highscoreEntry.score;
        highscoreTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString();
        
        transformList.Add(highscoreTransform);
        
    }

    private void AddHighscoreEntry(int score) {
        //Highscore Entry Created
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score };
        
        //Load Highscores
        Highscores highscores = new Highscores();
        highscores.highscoreEntries = FileHandler.ReadFromJSON<HighscoreEntry>(filename);
        
        //Add new Highscore Entry
        highscores.highscoreEntries.Add(highscoreEntry);
        
        //Save updated Highscore
        FileHandler.SaveToJSON<HighscoreEntry>(highscores.highscoreEntries, filename);
    }

    //Get Score from Run
    public int GetScore() {
        int reveivedScore = Score.curScore;

        return reveivedScore;
    }
    
    private class Highscores {
        public List<HighscoreEntry> highscoreEntries;
    }

    //One Highscore Entry
    [System.Serializable]
    private class HighscoreEntry {
        public int score;
    }

    //Button to get back to menu
    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }
    
}
