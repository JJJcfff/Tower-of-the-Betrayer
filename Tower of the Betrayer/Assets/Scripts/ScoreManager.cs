// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the score of the player
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int amount;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate ScoreManager", gameObject);
            Destroy(gameObject);
        }
    }
}
