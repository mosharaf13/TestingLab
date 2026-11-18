namespace TestingLab.Gameplay.Combat
{
    // Pattern: Interface Segregation (the "I" in SOLID). ContactDamage only needs
    // "can this thing be hit?" — not the whole HealthComponent API.

    public interface IDamageable
    {
        /// <returns>True if the hit landed; false if ignored (invulnerable, dead...).</returns>
        bool TryReceiveHit(int baseDamage);
    }
}
