using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    /// <summary>
    /// <c>Scroller</c> creates a rolling background effect for a raw image.
    /// </summary>
    public class Scroller : MonoBehaviour
    {
        [SerializeField] private RawImage _img;
        [SerializeField] private float _x, _y;

        void Update()
        {
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
        }
    }
}