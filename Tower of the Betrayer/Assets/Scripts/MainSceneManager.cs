using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public Button startGameButton;
    
    private void Start()
    {
        Debug.Log("MainSceneManager is Loaded!"); // Debug message
        if (startGameButton != null)
        {
            Debug.Log("StartGameButton is assigned.");
            startGameButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());
        }
        else
        {
            Debug.LogError("StartGameButton is NOT assigned in the Inspector!");
        }
    }
    
    private void OnButtonClicked()
    {
        Debug.Log("Button Click Registered! Calling StartGame()");
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("StartGame() Called! Attempting to Load Home Scene...");
        SceneManager.LoadScene("Home");
    }

    
    
}