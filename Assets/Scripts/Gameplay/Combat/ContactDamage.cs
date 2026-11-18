using UnityEngine;

namespace TestingLab.Gameplay.Combat
{
    /// <summary>
    /// Hurts any IDamageable that stays inside this object's trigger collider
    /// (the enemy's touch). How often it hurts is the receiver's decision.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class ContactDamage : MonoBehaviour
    {
        [SerializeField, Min(0)] private int damage = 10;

        public int Damage
        {
            get => damage;
            set => damage = Mathf.Max(0, value);
        }

        private void OnTriggerStay(Collider other)
        {
            var target = other.GetComponentInParent<IDamageable>();
            if (target != null)
                target.TryReceiveHit(damage);
        }
    }
}
