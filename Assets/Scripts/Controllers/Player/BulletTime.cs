using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Model.Player
{
    /// <summary>
    /// <c>BulletTime</c> is an ability that allows the player to slow down time.
    /// </summary>
    public class BulletTime
    {
        private float ResourceCost { get; }

        private readonly float _tickSpeed = 0.1f;

        private bool _bulletTimeActive;

        private readonly MonoBehaviour _behaviour;

        /// <summary>
        /// Constructor that initializes a <c>BulletTime</c> ability with a given <c>behaviour</c>.
        /// Also sets the resource cost.
        /// </summary>
        /// <param name="behaviour"><c>MonoBehaviour</c> instance for starting the <see cref="ManaDrain(Resource)"/> coroutine</param>
        public BulletTime(MonoBehaviour behaviour)
        {
            _behaviour = behaviour;
            _bulletTimeActive = false;
            ResourceCost = 20;
        }

        /// <summary>
        /// <c>Activate</c> activates the Bullet Time ability by lowering the in-game time scale in Unity.
        /// </summary>
        private void Activate()
        {
            Time.timeScale = 0.4f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            _bulletTimeActive = true;
        }

        /// <summary>
        /// <c>Deactivate</c> deactivates the Bullet Time ability by setting the in-game time scale in Unity back to normal.
        /// </summary>
        private void Deactivate()
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;
            _bulletTimeActive = false;
        }

        /// <summary>
        /// <c>Use</c> lets the player perform the ability and toggle between active and inactive Bullet Time.
        /// Once activated, the ability drains the player's mana resource (see <see cref=" Resource"/>) until it is depleted or the ability is turned off.
        /// </summary>
        /// <param name="playerModel">
        /// Player reference to access their mana resource
        /// </param>
        /// <returns>True if ability is activated; otherwise, false.</returns>
        public bool Use(PlayerModel playerModel)
        {
            if (!_bulletTimeActive)
            {
                Activate();
                _behaviour.StartCoroutine(ManaDrain(playerModel.Mana));
                return true;
            }

            Deactivate();
            return false;
        }

        /// <summary>
        /// <c>ManaDrain</c> coroutine that calls the Mana's <see cref="Resource.Drain(float, float)"/> method
        /// with a given <c>ResourceCost</c> and <c>_tickSpeed</c> while the Bullet Time ability is active
        /// and the player has enough Mana.
        /// </summary>
        /// <param name="resource">Mana resource of the player</param>
        /// <returns>
        /// <see cref="WaitForSeconds"/> delay if Bullet Time is active and there is enough mana; otherwise, <c>null</c>.
        /// </returns>
        protected virtual IEnumerator ManaDrain(Resource resource)
        {
            while (_bulletTimeActive)
            {
                if (resource.Value >= ResourceCost * _tickSpeed)
                {
                    resource.Drain(ResourceCost, _tickSpeed);
                }
                else
                {
                    Deactivate();
                }

                yield return new WaitForSeconds(_tickSpeed);
            }

            yield return null;
        }
    }
}