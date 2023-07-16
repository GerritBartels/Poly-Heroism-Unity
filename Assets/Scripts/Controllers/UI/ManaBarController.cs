using Model;

namespace Controllers.UI
{
    public class ManaBarController : ResourceBarController
    {
        protected override Resource Resource => Player.PlayerModel.Mana;
    }
}