using System;
using TestingLab.Core.Combat;
using TestingLab.Core.Common;
using TestingLab.Gameplay.Infrastructure;
using UnityEngine;

namespace TestingLab.Gameplay.Combat
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE → ADVANCED
    // Tested by:      Tests/PlayMode/Level2_Intermediate/ContactDamageTests.cs
    // Pattern:        Method injection (Construct) for MonoBehaviours
    //
    // MonoBehaviours can't have constructors, so dependencies arrive through
    // Construct(...). If nobody calls it, sensible real defaults are used.
    // Tests call Construct with a FakeRandomSource (no surprise crits) and a
    // FakeTimeProvider (control the invulnerability window).
    // ─────────────────────────────────────────────────────────────────────────

    [RequireComponent(typeof(HealthComponent))]
    public sealed class DamageReceiver : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(0)] private int armor;
        [SerializeField, Min(0f)] private float invulnerabilitySeconds = 0.75f;

        private HealthComponent _health;
        private DamageCalculator _calculator;
        private Cooldown _invulnerability;

        /// <summary>(damageDealt, wasCritical)</summary>
        public event Action<int, bool> Damaged;

        public int Armor
        {
            get => armor;
            set => armor = Mathf.Max(0, value);
        }

        /// <summary>Ignore all hits. Handy for tests and debugging.</summary>
        public bool GodMode { get; set; }

        public void Construct(DamageCalculator calculator, ITimeProvider time)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _invulnerability = new Cooldown(time ?? throw new ArgumentNullException(nameof(time)), invulnerabilitySeconds);
        }

        public bool TryReceiveHit(int baseDamage)
        {
            EnsureInitialised();
            if (GodMode || _health.Health == null || _health.Health.IsDead)
                return false;
            if (!_invulnerability.TryUse())
                return false;

            int damage = _calculator.Calculate(baseDamage, armor);
            _health.Health.TakeDamage(damage);
            Damaged?.Invoke(damage, _calculator.LastHitWasCrit);
            return true;
        }

        private void Awake() => EnsureInitialised();

        private void EnsureInitialised()
        {
            if (_health == null)
                _health = GetComponent<HealthComponent>();
            if (_calculator == null)
                Construct(new DamageCalculator(new UnityRandomSource()), new UnityTimeProvider());
        }
    }
}
