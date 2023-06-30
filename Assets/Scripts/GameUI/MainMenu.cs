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
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject settingsMenuUI;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Slider audioSlider;
        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private Toggle fullscreenToggle;

        private Resolution[] resolutions;

        private void Start()
        {
            // Load settings from PlayerPrefs
            float volume = PlayerPrefs.GetFloat("Volume", 0);
            audioSlider.value = volume;
            audioMixer.SetFloat("Volume", volume);

            int qualityIndex = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
            qualityDropdown.value = qualityIndex;
            QualitySettings.SetQualityLevel(qualityIndex);

            int isFullscreen = PlayerPrefs.GetInt("isFullscreen", -1);
            if (isFullscreen == -1)
            {
                fullscreenToggle.isOn = Screen.fullScreen;
            }
            else
            {
                fullscreenToggle.isOn = Convert.ToBoolean(isFullscreen);
                Screen.fullScreen = Convert.ToBoolean(isFullscreen);
            }

            float mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 0);
            mouseSensitivitySlider.value = mouseSensitivity;

            // Get available resolutions of current screen and add them as Dropdown options
            // & load resolution from PlayerPrefs
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            int resolutionIndex = PlayerPrefs.GetInt("resolutionIndex", -1);
            if (resolutionIndex != -1)
            {
                Resolution resolution = resolutions[resolutionIndex];
                Screen.SetResolution(width: resolution.width, height: resolution.height, fullscreen: Screen.fullScreen);
            }

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
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

        /// <summary>
        /// <c>SetVolume</c> is hooked up to the "Volume" slider and allows to control the ingame audio mixer volume level.
        /// </summary>
        /// <param name="volume">the new volume value</param>
        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("Volume", volume);
            PlayerPrefs.SetFloat("Volume", volume);
        }

        /// <summary>
        /// <c>SetQuality</c> is hooked up to the "Quality" dropdown and allows to control the ingame graphics quality.
        /// </summary>
        /// <param name="qualityIndex">Index of the new quality setting</param>
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt("Quality", qualityIndex);
        }

        /// <summary>
        /// <c>SetFullscreen</c> is hooked up to the "Fullscreen" toggle and allows to switch between windowed and fullscreen mode.
        /// </summary>
        /// <param name="isFullscreen">Indicator whether fullscreen mode is active</param>
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("isFullscreen", isFullscreen ? 1 : 0);
        }

        /// <summary>
        /// <c>SetResolution</c> is hooked up to the "Resolution" dropdown and allows to control the game resolution.
        /// </summary>
        /// <param name="resolutionIndex">Index of the new resolution setting</param>
        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(width: resolution.width, height: resolution.height, fullscreen: Screen.fullScreen);
            PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        }

        /// <summary>
        /// <c>SetMouseSensitivity</c> is hooked up to the "Mouse Sensitivity" slider and allows to control the ingame mouse sensitivity.
        /// </summary>
        /// <param name="mouseSensitivity">the new mouse sensitivity value</param>
        public void SetMouseSensitivity(float mouseSensitivity)
        {
            PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
        }
    }
}