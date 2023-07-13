using Model;

namespace Controllers.UI
{
    public class StaminaBarController : ResourceBarController
    {
        protected override Resource Resource => Player.PlayerModel.Stamina;
    }
}