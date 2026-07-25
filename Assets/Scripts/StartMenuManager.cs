using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void OnSettingsClick()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OnCreditsClick()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnExitClick()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
