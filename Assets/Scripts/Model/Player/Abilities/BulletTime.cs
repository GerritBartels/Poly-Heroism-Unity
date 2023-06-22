using UnityEngine;

namespace Model.Player.Abilities
{
    public class BulletTime : AbilityBase
    {
        private bool _bulletTimeActive;

        public BulletTime(float cooldown, float globalCooldown, float resourceCost, float blockMovementFor) : base(cooldown, globalCooldown, resourceCost, blockMovementFor)
        {
            _bulletTimeActive = false;
        }

        protected override Resource GetResource(PlayerModel playerModel)
        {
            return playerModel.Mana;
        }

        protected override bool PerformAbility(PlayerModel playerModel)
        {
            if (!_bulletTimeActive)
            {
                Time.timeScale = 0.4f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
            else
            {
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = 0.02f;
            }  

            _bulletTimeActive = !_bulletTimeActive;

            return true;
        }

        public override bool Use(PlayerModel playerModel)
        {
            var resource = GetResource(playerModel);
            if (resource.Value >= ResourceCost)
            {
                if (PerformAbility(playerModel))
                {
                    // TODO: drain Mana while skill is active 
                    resource.Drain(ResourceCost, 1f);
                    return true;
                }
            }

            return false;
        }
    }
}