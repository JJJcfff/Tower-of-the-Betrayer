// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updates score when associated object is destroyed.
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
