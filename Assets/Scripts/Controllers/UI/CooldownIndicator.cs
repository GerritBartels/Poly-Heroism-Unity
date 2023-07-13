using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers.Player;
using Model;
using Model.Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Controllers.UI
{
    public class CooldownIndicator : MonoBehaviour
    {
        private Cooldown GlobalCooldown => _playerController.PlayerModel.GlobalCooldown;
        private Cooldown AbilityCooldown => _playerController.Abilities[(int)ability].Cooldown;

        private PlayerController _playerController;

        [SerializeField] private Image cooldownImage;
        [SerializeField] private TMP_Text cooldownText;

        [SerializeField] private Abilities ability;

        private float _currentMaxCooldown = 0f;

        private void Awake()
        {
            _playerController = GameObject.Find("Player").GetComponentInParent<PlayerController>();
            Deactivate();
        }

        private void Deactivate()
        {
            _currentMaxCooldown = 0f;
            cooldownImage.fillAmount = 0f;
            cooldownText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (GlobalCooldown.IsCooldownActive() || AbilityCooldown.IsCooldownActive())
            {
                cooldownText.gameObject.SetActive(true);
                _currentMaxCooldown = new[]
                    { _currentMaxCooldown, GlobalCooldown.RemainingTime(), AbilityCooldown.RemainingTime() }.Max();
                var currentCooldown = Mathf.Max(GlobalCooldown.RemainingTime(), AbilityCooldown.RemainingTime());
                cooldownText.text = (Mathf.Round(currentCooldown * 10) / 10).ToString();
                cooldownImage.fillAmount = currentCooldown / _currentMaxCooldown;
            }
            else
            {
                Deactivate();
            }
        }
    }
}