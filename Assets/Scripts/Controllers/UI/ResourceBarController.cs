using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class ResourceBarController : MonoBehaviour
    {
        protected Resource Resource { get; set; }

        private Slider _slider;

        // Start is called before the first frame update
        protected void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        private void Update()
        {
            var fillValue = Resource.Value / Resource.MaxValue;
            _slider.value = fillValue;
        }
    }
}