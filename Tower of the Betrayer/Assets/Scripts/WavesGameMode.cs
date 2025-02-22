using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WavesGameMode : MonoBehaviour
{
    [SerializeField] Life playerLife;
    [SerializeField] Life playerBaseLife;
    
    void Start()
    {
        playerLife.onDeath.AddListener(OnPlayerDeathOrBaseDied);
        playerBaseLife.onDeath.AddListener(OnPlayerDeathOrBaseDied);
        EnemyManager.instance.onChanged.AddListener(CheckWinCondition);
        WaveManager.instance.onChanged.AddListener(CheckWinCondition);
    }
    
    void CheckWinCondition()
    {
        if (EnemyManager.instance.enemies.Count <= 0 && WaveManager.instance.waves.Count <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    private void Awake()
    {
        playerLife.onDeath.AddListener(OnPlayerDeathOrBaseDied);
    }

    void OnPlayerDeathOrBaseDied()
    {
        SceneManager.LoadScene("LoseScreen");
    }
}
