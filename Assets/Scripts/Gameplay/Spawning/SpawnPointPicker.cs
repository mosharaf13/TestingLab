using System;
using TestingLab.Core.Common;
using UnityEngine;

namespace TestingLab.Gameplay.Spawning
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (fake random sequences)
    // Tested by:      Tests/EditMode/Level2_Intermediate/SpawnPointPickerTests.cs
    //
    // Picks a random point in the arena that is not too close to the player
    // (spawning an enemy ON the player feels unfair). Tests feed it a scripted
    // sequence of "random" numbers to hit each branch.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class SpawnPointPicker
    {
        private readonly IRandomSource _random;
        private readonly Vector2 _halfExtents;
        private readonly float _height;

        public SpawnPointPicker(IRandomSource random, Vector2 halfExtents, float height = 0.5f)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            if (halfExtents.x <= 0f || halfExtents.y <= 0f)
                throw new ArgumentOutOfRangeException(nameof(halfExtents));

            _halfExtents = halfExtents;
            _height = height;
        }

        /// <summary>
        /// Tries up to maxAttempts random points and returns the first one at least
        /// minDistance from 'avoid'. If every attempt is too close, returns the
        /// farthest one found, so the game never gets stuck.
        /// </summary>
        public Vector3 Pick(Vector3 avoid, float minDistance, int maxAttempts = 10)
        {
            if (maxAttempts <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxAttempts));

            Vector3 best = default;
            float bestDistance = float.NegativeInfinity;

            for (int i = 0; i < maxAttempts; i++)
            {
                var candidate = new Vector3(
                    _random.Range(-_halfExtents.x, _halfExtents.x),
                    _height,
                    _random.Range(-_halfExtents.y, _halfExtents.y));

                float distance = Vector2.Distance(new Vector2(candidate.x, candidate.z), new Vector2(avoid.x, avoid.z));
                if (distance >= minDistance)
                    return candidate;

                if (distance > bestDistance)
                {
                    best = candidate;
                    bestDistance = distance;
                }
            }

            return best;
        }
    }
}
