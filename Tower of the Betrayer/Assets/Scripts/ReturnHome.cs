using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("Home");
    }
}
