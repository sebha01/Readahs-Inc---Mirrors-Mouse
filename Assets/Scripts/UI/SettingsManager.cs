using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public void OnBackClick()
    {
        SceneManager.LoadScene("Start");
    }
}
