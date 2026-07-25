using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    //Controls whether pause screen shows
    public static bool isPaused = false;
    //Player component references
    [Header("Player Component References")]
    [SerializeField]private PlayerInput playerInput;
    public GameObject pauseMenuUI;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Check if pause button pressed
        if (playerInput.actions["Pause"].WasPressedThisFrame())
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //Hide pause menu UI
        pauseMenuUI.SetActive(false);
        //Set time back so everything moves normally again
        Time.timeScale = 1.0f;
        //Make cursor disapear for camera movement
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void Pause()
    {
        // Show pause menu UI
        pauseMenuUI.SetActive(true);
        //Set time to 0 so everything freezes in place
        Time.timeScale = 0.0f;
        //Let the cursor show on the window so the user can interact with the pause menu
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void OnMainMenuClick()
    {
        //Hide pause menu UI
        pauseMenuUI.SetActive(false);
        //Set time back so everything moves normally again
        Time.timeScale = 1.0f;
        isPaused = false;
        SceneManager.LoadScene("Start");
    }
}
