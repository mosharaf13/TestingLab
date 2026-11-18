using UnityEngine;

namespace TestingLab.Gameplay.Player
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER → INTERMEDIATE
    // Tested by:      Tests/EditMode/Level2_Intermediate/MovementMathTests.cs
    //
    // A pure function: same inputs → same output, no hidden state. Pure functions
    // are the easiest code in the world to test, so we pull the maths out of
    // PlayerController.Update() into here.
    // ─────────────────────────────────────────────────────────────────────────

    public static class MovementMath
    {
        /// <summary>
        /// One frame of top-down movement on the XZ plane, clamped to an arena
        /// centred on the origin. Diagonal input is normalised so it isn't faster.
        /// </summary>
        public static Vector3 Step(Vector3 position, Vector2 input, float speed, float deltaTime, Vector2 arenaHalfExtents)
        {
            Vector2 direction = input.sqrMagnitude > 1f ? input.normalized : input;
            Vector3 next = position + new Vector3(direction.x, 0f, direction.y) * (speed * deltaTime);

            next.x = Mathf.Clamp(next.x, -arenaHalfExtents.x, arenaHalfExtents.x);
            next.z = Mathf.Clamp(next.z, -arenaHalfExtents.y, arenaHalfExtents.y);
            return next;
        }
    }
}
