using UnityEngine;

namespace TestingLab
{
    /// <summary>
    /// Keyboard movement for the demo scene (WASD / arrow keys).
    /// Deliberately kept tiny and untested: it only reads input and moves.
    /// All the logic worth testing lives in other classes ("humble object" pattern).
    /// </summary>
    public class DemoPlayerInput : MonoBehaviour
    {
        [SerializeField] private float speed = 6f;

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            var direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (direction.sqrMagnitude > 1f)
                direction.Normalize();
            transform.position += direction * (speed * Time.deltaTime);
#endif
        }
    }
}
