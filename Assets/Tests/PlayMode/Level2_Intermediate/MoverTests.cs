using System.Collections;
using NUnit.Framework;
using TestingLab.Gameplay.Enemies;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace TestingLab.Tests.PlayMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  — your first PLAY MODE tests
    // Code under test: Assets/Scripts/Gameplay/Enemies/Mover.cs
    //
    // Mover does its work in Update(), which only runs in Play Mode, frame by frame.
    // [UnityTest] methods return IEnumerator so they can WAIT:
    //     yield return null;   ← "let one frame pass, then continue the test"
    //
    // Play Mode tests are slower than Edit Mode tests. Use them only when you
    // need real frames, physics, coroutines or scenes.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class MoverTests
    {
        private GameObject _go;
        private Mover _mover;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("Mover");
            _mover = _go.AddComponent<Mover>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null)
                Object.Destroy(_go);
        }

        [UnityTest]
        public IEnumerator MoveTo_EventuallyArrivesAtTarget()
        {
            var target = new Vector3(2f, 0f, 0f);
            bool arrived = false;
            _mover.Arrived += () => arrived = true;
            _mover.Speed = 20f;

            _mover.MoveTo(target);

            // Always use a timeout: a broken test should fail, not hang the lab.
            yield return WaitFor.Condition(() => arrived, 2f, "Mover never arrived");

            Assert.That(_go.transform.position, Is.EqualTo(target).Using(Vector3EqualityComparer.Instance));
            Assert.That(_mover.IsMoving, Is.False);
        }

        [UnityTest]
        public IEnumerator MoveTo_MovesPartWayEachFrame()
        {
            _mover.Speed = 1f;

            _mover.MoveTo(new Vector3(100f, 0f, 0f));
            yield return null;
            yield return null;

            float x = _go.transform.position.x;
            Assert.That(x, Is.GreaterThan(0f));
            Assert.That(x, Is.LessThan(100f));
            Assert.That(_mover.IsMoving, Is.True);
        }

        [UnityTest]
        public IEnumerator ZeroSpeed_DoesNotMove()
        {
            _mover.Speed = 0f;

            _mover.MoveTo(new Vector3(5f, 0f, 0f));
            yield return WaitFor.Frames(5);

            Assert.That(_go.transform.position, Is.EqualTo(Vector3.zero).Using(Vector3EqualityComparer.Instance));
        }
    }
}
