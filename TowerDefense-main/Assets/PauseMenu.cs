using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsPanel;  // Reference to the settings panel

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
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
        pauseMenuUI.SetActive(false);
        settingsPanel.SetActive(false);  // Hide settings panel when resuming
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        settingsPanel.SetActive(false);  // Hide settings panel when pausing
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);   // Show the settings panel
        pauseMenuUI.SetActive(false);    // Hide the main pause menu
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);  // Hide the settings panel
        pauseMenuUI.SetActive(true);     // Show the main pause menu
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..");
        Application.Quit();
    }


}
