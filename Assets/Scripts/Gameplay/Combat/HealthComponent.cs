using System;
using TestingLab.Core.Combat;
using UnityEngine;

namespace TestingLab.Gameplay.Combat
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (MonoBehaviours in Edit Mode vs Play Mode)
    // Tested by:      Tests/EditMode/Level2_Intermediate/ComponentEditModeTests.cs
    //                 Tests/PlayMode/Level2_Intermediate/HealthComponentPlayModeTests.cs
    // Pattern:        Humble Object — a thin MonoBehaviour shell around Core's Health.
    // ─────────────────────────────────────────────────────────────────────────

    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maxHealth = 100;
        [SerializeField] private bool destroyOnDeath = true;

        public Health Health { get; private set; }
        public event Action Died;

        private void Awake()
        {
            if (Health == null)
                Initialise(maxHealth);
        }

        /// <summary>
        /// Creates a fresh Health. Awake does not run in Edit Mode tests, so tests
        /// call this directly; the Arena also calls it to apply shop upgrades.
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
