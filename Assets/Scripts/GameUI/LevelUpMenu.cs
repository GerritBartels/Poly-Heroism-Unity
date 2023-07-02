using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Model.Player;

namespace GameUI
{
    /// <summary>
    /// <c>LevelUpMenu</c> defines UI events for the level up game.
    /// This includes distributing open skill points and loading the next level.
    /// </summary>
    public class LevelUpMenu : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private TMP_Text finishedLevelText;
        [SerializeField] private TMP_Text strengthValueText;
        [SerializeField] private TMP_Text agilityValueText;
        [SerializeField] private TMP_Text intelligenceValueText;
        [SerializeField] private TMP_Text amountOfSkillPointsText;

        public int LVL = 0;

        public void Activate()
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            LVL = PlayerPrefs.GetInt("Level", 1);
            finishedLevelText.text = "Level " + LVL.ToString() + " finished!";
            amountOfSkillPointsText.text = "You have " +
                                           player.GetComponent<PlayerController>().PlayerModel.AttributePoints
                                               .ToString() +
                                           " Skill Point(s)";
            strengthValueText.text = player.GetComponent<PlayerController>().PlayerModel.Strength.ToString();
            agilityValueText.text = player.GetComponent<PlayerController>().PlayerModel.Agility.ToString();
            intelligenceValueText.text = player.GetComponent<PlayerController>().PlayerModel.Intelligence.ToString();
        }

        /// <summary>
        /// <c>IncreaseStrength</c> is hooked up to the strength's "Add" button which, when clicked,
        /// calls the <see cref="PlayerModel.IncreaseStrength"/> method and updates the respective text fields.
        /// </summary>
        public void IncreaseStrength()
        {
            player.GetComponent<PlayerController>().PlayerModel.IncreaseStrength();
            strengthValueText.text = player.GetComponent<PlayerController>().PlayerModel.Strength.ToString();
            amountOfSkillPointsText.color = Color.white;
            amountOfSkillPointsText.text = "You have " +
                                           player.GetComponent<PlayerController>().PlayerModel.AttributePoints
                                               .ToString() +
                                           " Skill Point(s)";
        }

        /// <summary>
        /// <c>IncreaseAgility</c> is hooked up to the agility's "Add" button which, when clicked,
        /// calls the <see cref="PlayerModel.IncreaseAgility"/> method and updates the respective text fields.
        /// </summary>
        public void IncreaseAgility()
        {
            player.GetComponent<PlayerController>().PlayerModel.IncreaseAgility();
            agilityValueText.text = player.GetComponent<PlayerController>().PlayerModel.Agility.ToString();
            amountOfSkillPointsText.color = Color.white;
            amountOfSkillPointsText.text = "You have " +
                                           player.GetComponent<PlayerController>().PlayerModel.AttributePoints
                                               .ToString() +
                                           " Skill Point(s)";
        }

        /// <summary>
        /// <c>IncreaseIntelligence</c> is hooked up to the intelligence's "Add" button which, when clicked,
        /// calls the <see cref="PlayerModel.IncreaseIntelligence"/> method and updates the respective text fields.
        /// </summary>
        public void IncreaseIntelligence()
        {
            player.GetComponent<PlayerController>().PlayerModel.IncreaseIntelligence();
            intelligenceValueText.text = player.GetComponent<PlayerController>().PlayerModel.Intelligence.ToString();
            amountOfSkillPointsText.color = Color.white;
            amountOfSkillPointsText.text = "You have " +
                                           player.GetComponent<PlayerController>().PlayerModel.AttributePoints
                                               .ToString() +
                                           " Skill Point(s)";
        }

        /// <summary>
        /// <c>NextLevel</c> is hooked up to the "Next Level" button which, when clicked,
        /// saves the player stats and loads the next level. 
        /// If the player still has unassigned skill points a warning will be displayed.
        /// </summary>
        public void NextLevel()
        {
            if (player.GetComponent<PlayerController>().PlayerModel.HasSkillPoints())
            {
                amountOfSkillPointsText.color = Color.red;
                amountOfSkillPointsText.text = "You still have to assign your " +
                                               player.GetComponent<PlayerController>().PlayerModel.AttributePoints
                                                   .ToString() +
                                               " Skill Point(s)";
            }
            else
            {
                PlayerPrefs.SetInt("Strength", player.GetComponent<PlayerController>().PlayerModel.Strength);
                PlayerPrefs.SetInt("Agility", player.GetComponent<PlayerController>().PlayerModel.Agility);
                PlayerPrefs.SetInt("Intelligence", player.GetComponent<PlayerController>().PlayerModel.Intelligence);
                PlayerPrefs.SetInt("Level", LVL + 1);
                Time.timeScale = 1f;
                Cursor.visible = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}