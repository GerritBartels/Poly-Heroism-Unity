namespace Controllers.Player
{
    public class MeleeAttackController : PlayerAttackControllerBase
    {
        private void Awake()
        {
            lifeSpan = 0.1f;
        }
    }
}