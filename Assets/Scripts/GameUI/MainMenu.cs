using Controllers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI
{
    /// <summary>
    /// <c>MainMenu</c> defines UI events for the main menu screen. 
    /// This includes functionality for the "Play", "Quit" and "Settings" buttons.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject settingsMenuUI;

        void OnApplicationQuit()
        {
            // Delete player stats and level
            PlayerPrefs.DeleteKey("Strength");
            PlayerPrefs.DeleteKey("Agility");
            PlayerPrefs.DeleteKey("Intelligence");
            PlayerPrefs.DeleteKey("Level");
            Debug.Log("Application ending after " + Time.time + " seconds");
        }

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

        /// <summary>
        /// <c>OpenSettings</c> is hooked up to the "Settings" button which, when clicked, opens the Settings menu and freezes time.
        /// </summary>
        public void OpenSettings()
        {
            player.GetComponent<PlayerController>().enabled = false;
            Time.timeScale = 0f;
            mainMenuUI.SetActive(false);
            settingsMenuUI.SetActive(true);
        }

        /// <summary>
        /// <c>CloseSettings</c> is hooked up to the "Back" button which, when clicked, closes the Settings menu and unfreezes time.
        /// </summary>
        public void CloseSettings()
        {
            player.GetComponent<PlayerController>().enabled = true;
            Time.timeScale = 1f;
            settingsMenuUI.SetActive(false);
            mainMenuUI.SetActive(true);
        }
    }
}