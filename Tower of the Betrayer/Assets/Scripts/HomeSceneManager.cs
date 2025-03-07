using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneManager : MonoBehaviour
{
    public Button startGameButton;

    private void Start()
    {
        Debug.Log("HomeSceneManager is Loaded!"); // Debug message
        if (startGameButton != null)
        {
            Debug.Log("StartGameButton is assigned.");
            startGameButton.onClick.AddListener(() => StartGame());
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
        Debug.Log("StartGame() Called! Attempting to Load Scene...");
        SceneManager.LoadScene("Game");
    }
}
