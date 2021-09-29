using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelModifierScript : MonoBehaviour
{
    public int power;
    public static int moveSpeedModifier =0;
    public static int enemyDamageModifier=0;
    public static int healthBoost = 0;


    //public int highestCurrentLevel = ;

    public static void IncreaseModifier()
    {
        moveSpeedModifier += 1;
        enemyDamageModifier += 1;
        healthBoost += 1;
       // Debug.Log("moveSpeedModifier: " + moveSpeedModifier);
      //  Debug.Log("enemyDamageModifier: " + enemyDamageModifier);

    }
    public static void DecreaseModifier()
    {
        moveSpeedModifier =0;
        enemyDamageModifier =0;
        healthBoost = 0;
    }
    public void NextLevel()
    {
       // PlayerPrefs.SetInt()
    }
}
