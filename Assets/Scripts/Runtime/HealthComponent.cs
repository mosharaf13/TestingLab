using System;
using UnityEngine;

namespace TestingLab
{
    /// <summary>
    /// Thin MonoBehaviour wrapper around Health. Logs and (optionally) destroys itself on death.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private bool destroyOnDeath = true;

        public Health Health { get; private set; }
        public event Action Died;

        private void Awake()
        {
            Initialise(maxHealth);
        }

        /// <summary>
        /// Awake does not run in Edit Mode tests, so tests call this directly.
        /// </summary>
        public void Initialise(int max)
        {
            if (Health != null)
                Health.Died -= HandleDied;

            maxHealth = max;
            Health = new Health(max);
            Health.Died += HandleDied;
        }

        public void SetDestroyOnDeath(bool value) => destroyOnDeath = value;

        private void HandleDied()
        {
            Debug.Log($"{name} died");
            Died?.Invoke();
            if (destroyOnDeath)
                Destroy(gameObject);
        }
    }
}
