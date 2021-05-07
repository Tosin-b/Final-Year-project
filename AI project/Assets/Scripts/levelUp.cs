using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class levelUp : MonoBehaviour
{
    public  Text currentGameLevel;
    public static int levelIncrement = 1;
    // Start is called before the first frame update
    void Start()
    {
        currentGameLevel.text = "LEVEL" + levelIncrement.ToString();
    }

    // Update is called once per frame
   
    public  void LevelingUp()
    {
        //currentScoreText.text = "POINTS: " + score.ToString();
        currentGameLevel.text = "LEVEL: " + levelIncrement.ToString();
        levelIncrement += 1;
    }
    public  void LevelDecrement()
    {
        currentGameLevel.text = "LEVEL: " + levelIncrement.ToString();
        levelIncrement = 1;
        //if (levelIncrement < 1)
        //{
        //    levelIncrement = 1;
        //    currentGameLevel.text = "LEVEL: " + levelIncrement.ToString();
        //}


    }
}
