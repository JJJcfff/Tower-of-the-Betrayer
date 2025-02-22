using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnDeath : MonoBehaviour
{

    public int amount;

    private void Awake()
    {
       
    }
    
    void GivePoints()
    {
        ScoreManager.instance.amount += amount;
    }
}
