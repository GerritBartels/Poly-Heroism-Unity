using Controllers.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class BossHealthBarController : MonoBehaviour
    {
        public AbstractEnemyController Boss { get; set; }

        private Slider _slider;
        private TMP_Text _valueText;

        // Start is called before the first frame update
        protected void Awake()
        {
            _slider = GetComponent<Slider>();
            _valueText = GetComponentInChildren<TMP_Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            var resource = Boss.GetEnemy().Health;
            _slider.value = resource.Value / resource.MaxValue;
            _valueText.text = (Mathf.Round(resource.Value * 10) / 10).ToString();
        }
    }
}