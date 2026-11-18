using UnityEngine;

namespace TestingLab.Gameplay.Pickups
{
    /// <summary>Cosmetic only: spins an object. Not worth a test (no logic to break).</summary>
    public sealed class Spinner : MonoBehaviour
    {
        [SerializeField] private float degreesPerSecond = 90f;

        private void Update()
        {
            transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f, Space.World);
        }
    }
}
