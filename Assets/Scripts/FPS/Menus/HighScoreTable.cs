using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HighScoreTable : MonoBehaviour {

    private Transform highscoreContainer;
    private Transform highscoreTemplate;
    
    //References to Transforms
    private void Awake() {
        highscoreContainer = transform.Find("HighscoreContainer");
        highscoreTemplate = highscoreContainer.Find("HighscoreTemplate");
        
        highscoreTemplate.gameObject.SetActive(false);

        float templateHeight = 50f;
        for (int i = 0; i < 10; i++) {
            Transform highscoreTransform = Instantiate(highscoreTemplate, highscoreContainer);
            RectTransform highscoreRectTransform = highscoreTransform.GetComponent<RectTransform>();
            highscoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            highscoreTransform.gameObject.SetActive(true);

            int rank = i + 1;
            string rankString;
            switch (rank) {
                default:
                    rankString = rank + "."; break;
            }
            highscoreTransform.Find("RankText").GetComponent<Text>().text = rankString;

            var score = Random.Range(0, 10000);
            highscoreTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString();


        }
    }
}
