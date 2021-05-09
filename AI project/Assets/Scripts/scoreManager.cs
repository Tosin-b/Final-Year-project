using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    

    public Text currentScoreText;
    public Text HighScoreText;
   public int score = 0;
    int highScore = 0;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
      
        highScore = PlayerPrefs.GetInt("highscore", 0);
        currentScoreText.text =  ":  POINTS: " + score.ToString();
        HighScoreText.text = "HIGHSCORE: " + highScore.ToString();
    }

    // Update is called once per frame
    public void Addscore()
    {
        score += 10  ;
        Debug.Log("score new"+score);
        currentScoreText.text =   "POINTS: " + score.ToString();
        if(highScore < score)
        PlayerPrefs.SetInt("highscore", score);
    }
}
