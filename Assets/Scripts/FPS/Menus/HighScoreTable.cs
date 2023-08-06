using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour {

    private Transform highscoreContainer;
    private Transform highscoreTemplate;

    private List<Transform> highscoreEntriesTransforms;

    private void Start() {

        Cursor.lockState = CursorLockMode.None;
        
        //References to Transforms
        highscoreContainer = transform.Find("HighscoreContainer");
        highscoreTemplate = highscoreContainer.Find("HighscoreTemplate");
        
        highscoreTemplate.gameObject.SetActive(false);

        AddHighscoreEntry(GetScore());
        //AddHighscoreEntry(10);
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
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
        
        if (highscores.highscoreEntries.Count > 10) {
            for (int h = highscores.highscoreEntries.Count; h > 10; h--) {
                highscores.highscoreEntries.RemoveAt(10);
            }
        }

        highscoreEntriesTransforms = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntries) {
            CreateHighscoreEntryTransform(highscoreEntry, highscoreContainer, highscoreEntriesTransforms);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 50f;
        
        Transform highscoreTransform = Instantiate(highscoreTemplate, container);
        RectTransform highscoreRectTransform = highscoreTransform.GetComponent<RectTransform>();
        highscoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        highscoreTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
            default:
                rankString = rank + "."; break;
        }
        highscoreTransform.Find("RankText").GetComponent<Text>().text = rankString;

        var score = highscoreEntry.score;
        highscoreTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString();
        
        transformList.Add(highscoreTransform);
        
    }

    private void AddHighscoreEntry(int score) {
        //Highscore Entry Created
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score };
        
        //Load Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        //Add new Highscore Entry
        highscores.highscoreEntries.Add(highscoreEntry);
        
        //Save updated Highscore
        string json = JsonUtility.ToJson(highscores);

        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

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

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }
    
}