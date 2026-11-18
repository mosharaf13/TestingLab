using System;
using UnityEngine;

namespace TestingLab.Gameplay.Enemies
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (boundary values, state over time)
    // Tested by:      Tests/EditMode/Level2_Intermediate/ChaseBrainTests.cs
    //
    // Enemy AI decisions as plain C#. It uses Unity's Vector3 (a struct), which is
    // fine in Edit Mode tests: only scenes, frames and physics need Play Mode.
    //
    //   Idle/Returning ── player within detectRange ──▶ Chasing
    //   Chasing ──────── player beyond loseRange ─────▶ Returning (walk home)
    //   Returning ────── reached home ────────────────▶ Idle
    //
    // loseRange > detectRange on purpose ("hysteresis"): otherwise an enemy at
    // exactly the edge would flip between chasing and not chasing every frame.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class ChaseBrain
    {
        private const float ArrivedDistance = 0.1f;

        public float DetectRange { get; }
        public float LoseRange { get; }
        public ChaseState State { get; private set; } = ChaseState.Idle;

        public ChaseBrain(float detectRange, float loseRange)
        {
            if (detectRange <= 0f)
                throw new ArgumentOutOfRangeException(nameof(detectRange));
            if (loseRange < detectRange)
                throw new ArgumentException("loseRange must be at least detectRange.", nameof(loseRange));

            DetectRange = detectRange;
            LoseRange = loseRange;
        }

        /// <returns>Where the enemy should walk this frame.</returns>
        public Vector3 ChooseDestination(Vector3 self, Vector3 player, Vector3 home)
        {
            float distanceToPlayer = FlatDistance(self, player);

            switch (State)
            {
                case ChaseState.Chasing when distanceToPlayer > LoseRange:
                    State = ChaseState.Returning;
                    break;
                case ChaseState.Idle when distanceToPlayer <= DetectRange:
                case ChaseState.Returning when distanceToPlayer <= DetectRange:
                    State = ChaseState.Chasing;
                    break;
                case ChaseState.Returning when FlatDistance(self, home) <= ArrivedDistance:
                    State = ChaseState.Idle;
                    break;
            }

            Vector3 destination = State == ChaseState.Chasing ? player : home;
            destination.y = self.y; // stay on the ground
            return destination;
        }

        private static float FlatDistance(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
