using Controllers;
using Controllers.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Model.Player;
using UnityEngine.Serialization;

namespace Controllers.UI.Menu
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

        private int _lvl = 0;
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = player.GetComponent<PlayerController>();
        }

        public void Activate()
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            _lvl = PlayerPrefs.GetInt("Level", 1);
            finishedLevelText.text = "Level " + _lvl + " finished!";
            amountOfSkillPointsText.text = "You have " +
                                           _playerController.PlayerModel.AttributePoints +
                                           " Skill Point(s)";
            strengthValueText.text = _playerController.PlayerModel.Strength.ToString();
            agilityValueText.text = _playerController.PlayerModel.Agility.ToString();
            intelligenceValueText.text = _playerController.PlayerModel.Intelligence.ToString();
        }

        /// <summary>
        /// <c>IncreaseStrength</c> is hooked up to the strength's "Add" button which, when clicked,
        /// calls the <see cref="PlayerModel.IncreaseStrength"/> method and updates the respective text fields.
        /// </summary>
        public void IncreaseStrength()
        {
            _playerController.PlayerModel.IncreaseStrength();
            strengthValueText.text = _playerController.PlayerModel.Strength.ToString();
            amountOfSkillPointsText.color = Color.white;
            amountOfSkillPointsText.text = "You have " +
                                           _playerController.PlayerModel.AttributePoints +
                                           " Skill Point(s)";
        }

        /// <summary>
        /// <c>IncreaseAgility</c> is hooked up to the agility's "Add" button which, when clicked,
        /// calls the <see cref="PlayerModel.IncreaseAgility"/> method and updates the respective text fields.
        /// </summary>
        public void IncreaseAgility()
        {
            _playerController.PlayerModel.IncreaseAgility();
            agilityValueText.text = _playerController.PlayerModel.Agility.ToString();
            amountOfSkillPointsText.color = Color.white;
            amountOfSkillPointsText.text = "You have " +
                                           _playerController.PlayerModel.AttributePoints +
                                           " Skill Point(s)";
        }

        /// <summary>
        /// <c>IncreaseIntelligence</c> is hooked up to the intelligence's "Add" button which, when clicked,
        /// calls the <see cref="PlayerModel.IncreaseIntelligence"/> method and updates the respective text fields.
        /// </summary>
        public void IncreaseIntelligence()
        {
            _playerController.PlayerModel.IncreaseIntelligence();
            intelligenceValueText.text = _playerController.PlayerModel.Intelligence.ToString();
            amountOfSkillPointsText.color = Color.white;
            amountOfSkillPointsText.text = "You have " +
                                           _playerController.PlayerModel.AttributePoints +
                                           " Skill Point(s)";
        }

        /// <summary>
        /// <c>NextLevel</c> is hooked up to the "Next Level" button which, when clicked,
        /// saves the player stats and loads the next level. 
        /// If the player still has unassigned skill points a warning will be displayed.
        /// </summary>
        public void NextLevel()
        {
            if (_playerController.PlayerModel.HasSkillPoints())
            {
                amountOfSkillPointsText.color = Color.red;
                amountOfSkillPointsText.text = "You still have to assign your " +
                                               _playerController.PlayerModel.AttributePoints +
                                               " Skill Point(s)";
            }
            else
            {
                PlayerPrefs.SetInt("Strength", _playerController.PlayerModel.Strength);
                PlayerPrefs.SetInt("Agility", _playerController.PlayerModel.Agility);
                PlayerPrefs.SetInt("Intelligence", _playerController.PlayerModel.Intelligence);
                PlayerPrefs.SetInt("Level", _lvl + 1);
                Time.timeScale = 1f;
                Cursor.visible = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}