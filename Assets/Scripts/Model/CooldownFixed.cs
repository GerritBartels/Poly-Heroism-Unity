namespace Model
{
    public class CooldownFixed : Cooldown
    {
        public float Duration { get; }

        public CooldownFixed(float duration)
        {
            Duration = duration;
        }

        public void Apply()
        {
            base.Apply(Duration);
        }
    }
}