using System.Collections;
using NUnit.Framework;
using TestingLab.Gameplay.Enemies;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level2_Intermediate
{
    // Learning level: INTERMEDIATE
    // ChaseBrainTests (Edit Mode) prove the DECISIONS are right. This Play Mode
    // test proves EnemyController actually ACTS on them, frame by frame.
    // We need only one or two of these, because the edge cases are already covered.

    [Category(TestCategory.Intermediate)]
    public class EnemyChaseTests
    {
        private GameObject _enemy;
        private GameObject _target;

        [SetUp]
        public void SetUp()
        {
            _enemy = new GameObject("Enemy");
            _enemy.AddComponent<Mover>();
            _target = new GameObject("Target");
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_enemy);
            Object.Destroy(_target);
        }

        [UnityTest]
        public IEnumerator NearbyTarget_IsChasedAndReached()
        {
            var controller = _enemy.AddComponent<EnemyController>();
            _target.transform.position = new Vector3(3f, 0f, 0f);
            controller.Configure(_target.transform, speed: 10f);

            yield return WaitFor.Condition(
                () => Vector3.Distance(_enemy.transform.position, _target.transform.position) < 0.1f,
                2f, "enemy never reached the target");

            Assert.That(controller.State, Is.EqualTo(ChaseState.Chasing));
        }

        [UnityTest]
        public IEnumerator FarTarget_IsIgnored()
        {
            var controller = _enemy.AddComponent<EnemyController>();
            _target.transform.position = new Vector3(50f, 0f, 0f);
            controller.Configure(_target.transform, speed: 10f);

            yield return WaitFor.Frames(10);

            Assert.That(_enemy.transform.position, Is.EqualTo(Vector3.zero));
            Assert.That(controller.State, Is.EqualTo(ChaseState.Idle));
        }
    }
}
