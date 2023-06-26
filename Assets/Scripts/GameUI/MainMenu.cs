using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    /// <summary>
    /// <c>MainMenu</c> defines UI events for the main menu screen.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// <c>PlayGame</c> is hooked up to the "Play" button which, when clicked, loads the next scene. 
        /// </summary>
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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