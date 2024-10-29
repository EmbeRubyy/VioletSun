using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverMenu; // Reference to the actual Game Over UI GameObject
    public PauseMenu pauseMenu; // Reference to the PauseMenu script

    void Start()
    {
        // Ensure the Game Over Menu is inactive at the start
        gameOverMenu.SetActive(false);
    }

    public void ShowGameOver()
    {
        Debug.Log("Displaying Game Over Menu"); // Debug message for visibility
        gameOverMenu.SetActive(true); // Activate the Game Over Menu
        Time.timeScale = 0f; // Freeze the game time

        // Ensure the game is unpaused
        PauseMenu.isPaused = false; 
    }

    public void RestartButton()
    {
        Debug.Log("Restarting Game..."); // Debug message for visibility
        
        // Unfreeze the game time and resume game
        Time.timeScale = 1f; // Unfreeze the game time
        SceneManager.LoadScene(1); // Reload the current scene
    }

    public void ExitButton()
    {
        Debug.Log("Exiting to Main Menu..."); // Debug message for visibility
        Time.timeScale = 1f; // Ensure game time is normal when exiting
        SceneManager.LoadScene(0); // Load the main menu scene (ensure index matches your build settings)
    }
}
