using TMPro;
using Unity.VisualScripting;

namespace Model
{
    public class Resource
    {
        public Resource(float regenerationRate, float maxValue = 100f, float value = 100f)
        {
            _regenerationRate = regenerationRate;
            MaxValue = maxValue;
            Value = value;
        }

        private readonly float _regenerationRate;
        public float MaxValue { get; }

        private float _value;
        public const float MinValue = 0f;

        public float Value
        {
            get => _value;
            set
            {
                if (value > MaxValue)
                {
                    _value = MaxValue;
                }
                else if (value < MinValue)
                {
                    _value = MinValue;
                }
                else
                {
                    _value = value;
                }
            }
        }

        public bool Empty()
        {
            return Value <= MinValue;
        }

        public void Regenerate(float duration)
        {
            Value += _regenerationRate * duration;
        }
    }
}