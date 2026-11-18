using NUnit.Framework;
using TestingLab.Gameplay.Player;
using TestingLab.Tests.Shared;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Gameplay/Player/MovementMath.cs
    //
    // Unity types like Vector3 work fine in Edit Mode. Compare vectors with
    // Vector3EqualityComparer: floats add tiny rounding errors, so exact == fails.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class MovementMathTests
    {
        private static readonly Vector2 BigArena = new Vector2(100f, 100f);

        [Test]
        public void Step_MovesInInputDirectionAtSpeed()
        {
            var next = MovementMath.Step(Vector3.zero, Vector2.right, speed: 5f, deltaTime: 0.5f, BigArena);

            Assert.That(next, Is.EqualTo(new Vector3(2.5f, 0f, 0f)).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        public void Step_InputYMovesAlongWorldZ()
        {
            var next = MovementMath.Step(Vector3.zero, Vector2.up, 1f, 1f, BigArena);

            Assert.That(next, Is.EqualTo(new Vector3(0f, 0f, 1f)).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        public void Step_DiagonalIsNotFaster()
        {
            var next = MovementMath.Step(Vector3.zero, new Vector2(1f, 1f), 1f, 1f, BigArena);

            Assert.That(new Vector2(next.x, next.z).magnitude, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void Step_IsClampedInsideTheArena()
        {
            var start = new Vector3(8.9f, 1f, 0f);

            var next = MovementMath.Step(start, Vector2.right, 10f, 1f, new Vector2(9f, 9f));

            Assert.That(next.x, Is.EqualTo(9f));
            Assert.That(next.y, Is.EqualTo(1f), "height is never changed");
        }

        [Test]
        public void Step_NoInput_StaysPut()
        {
            var start = new Vector3(3f, 1f, -2f);

            var next = MovementMath.Step(start, Vector2.zero, 10f, 1f, BigArena);

            Assert.That(next, Is.EqualTo(start).Using(Vector3EqualityComparer.Instance));
        }
    }
}
