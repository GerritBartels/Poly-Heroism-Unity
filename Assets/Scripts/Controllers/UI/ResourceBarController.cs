using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

namespace Controllers.UI
{
    public class ResourceBarController : MonoBehaviour
    {
        protected Resource Resource { get; set; }

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
            _slider.value = Resource.Value / Resource.MaxValue;
            _valueText.text = (Mathf.Round(Resource.Value * 10) / 10).ToString();
        }
    }
}