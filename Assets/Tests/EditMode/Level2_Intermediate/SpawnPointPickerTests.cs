using NUnit.Framework;
using TestingLab.Gameplay.Spawning;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;
using UnityEngine;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Gameplay/Spawning/SpawnPointPicker.cs
    //
    // Topic: a SEQUENCE of fake random numbers drives the code down a specific
    //        path ("first try is too close, second try is fine").
    //        Random value 0.0 → left/back edge, 0.5 → centre, 1.0 → right/front.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class SpawnPointPickerTests
    {
        private static readonly Vector2 Arena = new Vector2(10f, 10f);

        [Test]
        public void Pick_MapsRandomValuesOntoTheArena()
        {
            var random = new FakeRandomSource().Enqueue(0f, 0.75f);
            var picker = new SpawnPointPicker(random, Arena, height: 1f);

            var point = picker.Pick(avoid: new Vector3(100f, 0f, 100f), minDistance: 0f);

            Assert.That(point, Is.EqualTo(new Vector3(-10f, 1f, 5f)));
        }

        [Test]
        public void Pick_RetriesWhenTooCloseToPlayer()
        {
            var random = new FakeRandomSource().Enqueue(
                0.5f, 0.5f,   // attempt 1: centre of the arena, right on the player
                0.9f, 0.5f);  // attempt 2: x = 8, far enough
            var picker = new SpawnPointPicker(random, Arena);

            var point = picker.Pick(avoid: Vector3.zero, minDistance: 3f);

            Assert.That(point.x, Is.EqualTo(8f).Within(0.0001f));
            Assert.That(random.CallCount, Is.EqualTo(4), "two attempts, two numbers each");
        }

        [Test]
        public void Pick_WhenEveryAttemptIsTooClose_ReturnsTheFarthest()
        {
            var random = new FakeRandomSource().Enqueue(0.5f, 0.5f, 0.6f, 0.5f, 0.55f, 0.5f);
            var picker = new SpawnPointPicker(random, Arena);

            var point = picker.Pick(avoid: Vector3.zero, minDistance: 50f, maxAttempts: 3);

            Assert.That(point.x, Is.EqualTo(2f).Within(0.0001f));
        }
    }
}
