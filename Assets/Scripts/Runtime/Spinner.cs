using UnityEngine;

namespace TestingLab
{
    /// <summary>Cosmetic: spins an object around the world up axis.</summary>
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float degreesPerSecond = 90f;

        private void Update()
        {
            transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f, Space.World);
        }
    }
}
