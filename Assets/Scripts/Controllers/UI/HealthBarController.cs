using Model;

namespace Controllers.UI
{
    public class HealthBarController : ResourceBarController
    {
        protected override Resource Resource => Player.PlayerModel.Health;
    }
}