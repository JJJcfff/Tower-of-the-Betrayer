using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainButton : MonoBehaviour
{
    public void LoadMainScene()
    {
        GameManager.Instance.LoadMainScene();
    }
}
