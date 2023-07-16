using TMPro;
using UnityEngine;

namespace Controllers.UI
{
    public class LvlDisplayController : MonoBehaviour
    {
        [SerializeField] private TMP_Text lvlText;

        private void Awake()
        {
            lvlText.text = "LVL: " + PlayerPrefs.GetInt("Level", 1);
        }
    }
}