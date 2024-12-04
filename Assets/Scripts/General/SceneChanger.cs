using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}