using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Core game state
    public int currentFloor = 1;
    public int score = 0;
    
    // Weapon state
    public bool hasSword = false;
    public bool hasStaff = false;
    
    // References to other managers
    public ScoreManager scoreManager;
    public EnemyManager enemyManager;
    public WaveManager waveManager;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeGame()
    {
        currentFloor = 1;
        score = 0;
        hasSword = false;
        hasStaff = false;
    }

    public void StartNewGame()
    {
        InitializeGame();
        SceneManager.LoadScene("Home");
    }

    public void CompleteFloor(bool success)
    {
        if (success)
        {
            currentFloor++;
            // Save any floor completion rewards
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("Home");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}