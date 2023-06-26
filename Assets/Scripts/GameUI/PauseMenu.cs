using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    /// <summary>
    /// <c>PauseMenu</c> defines UI events for the pause menu screen.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private GameObject player;

        public static bool GameIsPaused = false;

        private void Update()
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

        /// <summary>
        /// <c>Pause</c> activates the pause menu and pauses the game by freezing time. 
        /// </summary>
        private void Pause()
        {
            // Disable PlayerController script to avoid casting abilities through Key events triggered during the pause screen
            player.GetComponent<PlayerController>().enabled = false;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        /// <summary>
        /// <c>Resume</c> deactivates the pause menu and resumes the game by unfreezing time.
        /// It is also hooked up to the "Resume" button.
        /// </summary>
        public void Resume()
        {
            player.GetComponent<PlayerController>().enabled = true;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        /// <summary>
        /// <c>LoadMenu</c> is hooked up to the "Menu" button which, when clicked, loads the previous scene.
        /// </summary>
        public void LoadMenu()
        {
            GameIsPaused = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        /// <summary>
        /// <c>QuitGame</c> is hooked up to the "Quit" button which, when clicked, closes the application.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}