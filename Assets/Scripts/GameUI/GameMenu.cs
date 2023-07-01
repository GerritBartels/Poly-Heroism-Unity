using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    /// <summary>
    /// <c>GameMenu</c> defines UI events for the main game. This includes a pause, settings and game over menu.
    /// </summary>
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private GameObject gameOverMenuUI;
        [SerializeField] private GameObject playerResourcesUI;
        [SerializeField] private GameObject playerAbilitiesUI;
        [SerializeField] private GameObject settingsMenuUi;
        [SerializeField] private GameObject player;

        public static bool GameIsPaused = false;
        private Camera _cam;

        private void Awake()
        {
            Cursor.visible = false;
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && player.GetComponent<PlayerController>().PlayerModel.IsAlive)
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
        /// Also deactivates the player's resource and ability UI.
        /// </summary>
        private void Pause()
        {
            // Disable PlayerController script to avoid casting abilities through Key events triggered during the pause screen
            player.GetComponent<PlayerController>().enabled = false;
            Cursor.visible = true;
            playerAbilitiesUI.SetActive(false);
            playerResourcesUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        /// <summary>
        /// <c>Resume</c> deactivates the pause menu and resumes the game by unfreezing time.
        /// It is also hooked up to the "Resume" button and activates the player's resource and ability UI.
        /// </summary>
        public void Resume()
        {
            player.GetComponent<PlayerController>().enabled = true;
            Cursor.visible = false;
            pauseMenuUI.SetActive(false);
            settingsMenuUi.SetActive(false);
            playerAbilitiesUI.SetActive(true);
            playerResourcesUI.SetActive(true);
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

        /// <summary>
        /// <c>GameOver</c> activates the game over menu and triggers the camera's game over animation.
        /// Also deactivates the player's resource and ability UI.
        /// </summary>
        public void GameOver()
        {
            Cursor.visible = true;
            playerAbilitiesUI.SetActive(false);
            playerResourcesUI.SetActive(false);
            gameOverMenuUI.SetActive(true);
            _cam.GetComponent<Animator>().SetTrigger("dead");
        }

        /// <summary>
        /// <c>Restart</c> is hooked up to the "Restart" button which, when clicked, restarts the level.
        /// </summary>
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}