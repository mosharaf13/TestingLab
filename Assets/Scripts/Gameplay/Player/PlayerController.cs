using UnityEngine;

namespace TestingLab.Gameplay.Player
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (Play Mode tests)
    // Tested by:      Tests/PlayMode/Level2_Intermediate/PlayerControllerTests.cs
    //
    // Thin MonoBehaviour: reads IPlayerInput, asks MovementMath where to go.
    // A kinematic Rigidbody lets it trigger coins and enemies without physics forces.
    // ─────────────────────────────────────────────────────────────────────────

    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 6f;
        [SerializeField] private Vector2 arenaHalfExtents = new Vector2(9f, 9f);

        /// <summary>Where movement comes from. Tests replace this with a scripted input.</summary>
        public IPlayerInput InputSource { get; set; }

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = Mathf.Max(0f, value);
        }

        public Vector2 ArenaHalfExtents
        {
            get => arenaHalfExtents;
            set => arenaHalfExtents = value;
        }

        private void Awake()
        {
            InputSource ??= new KeyboardPlayerInput();
        }

        private void Update()
        {
            Vector2 move = InputSource?.Move ?? Vector2.zero;
            transform.position = MovementMath.Step(transform.position, move, moveSpeed, Time.deltaTime, arenaHalfExtents);

            if (move.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(new Vector3(move.x, 0f, move.y));
        }
    }
}
