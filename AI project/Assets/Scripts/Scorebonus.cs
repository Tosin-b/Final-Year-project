using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorebonus : MonoBehaviour
{
    public int ScorebonusPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GiveEnenyMorePoints()
    {
        ScorebonusPoints += 5;
    }

  
}
