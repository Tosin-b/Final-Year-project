using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelModifierScript : MonoBehaviour
{
    public int power;
    public static int moveSpeedModifier;
    public static int enemyDamageModifier;




    public static void IncreaseModifier()
    {
        moveSpeedModifier += 1;
        enemyDamageModifier += 1;
        Debug.Log("moveSpeedModifier: " + moveSpeedModifier);
        Debug.Log("enemyDamageModifier: " + enemyDamageModifier);

    }
    public static void DecreaseModifier()
    {
        moveSpeedModifier =0;
        enemyDamageModifier =0;
    }
}
