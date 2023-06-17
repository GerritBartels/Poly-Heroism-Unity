namespace Model.Abilities
{
    public class ScatterShot : AbilityBase
    {
        protected override bool PerformAbility(Player player)
        {
            throw new System.NotImplementedException();
        }

        protected override Resource GetResource(Player player)
        {
            return player.Mana;
        }

        public ScatterShot() : base(5, 0.5f, 20)
        {
        }
    }
}