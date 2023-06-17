namespace Model.Abilities
{
    public class RangedAttack : AbilityBase
    {
        public RangedAttack() : base(0f, 0.2f, 2f)
        {
        }

        protected override bool PerformAbility(Player player)
        {
            throw new System.NotImplementedException();
        }

        protected override Resource GetResource(Player player)
        {
            return player.Mana;
        }
    }
}