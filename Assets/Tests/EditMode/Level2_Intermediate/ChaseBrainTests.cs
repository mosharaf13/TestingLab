using System;
using NUnit.Framework;
using TestingLab.Gameplay.Enemies;
using TestingLab.Tests.Shared;
using UnityEngine;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Gameplay/Enemies/ChaseBrain.cs
    // In the game:     red enemies start chasing you when you get close.
    //
    // Topic: AI is just decisions. Pulled out of the MonoBehaviour, every
    //        decision can be tested in microseconds, including tricky edges.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class ChaseBrainTests
    {
        private static readonly Vector3 Home = Vector3.zero;
        private ChaseBrain _brain;

        [SetUp]
        public void SetUp() => _brain = new ChaseBrain(detectRange: 5f, loseRange: 8f);

        [Test]
        public void PlayerFarAway_StaysIdleAtHome()
        {
            var destination = _brain.ChooseDestination(Home, new Vector3(20f, 0f, 0f), Home);

            Assert.That(_brain.State, Is.EqualTo(ChaseState.Idle));
            Assert.That(destination, Is.EqualTo(Home));
        }

        [Test]
        public void PlayerExactlyAtDetectRange_StartsChase()
        {
            var player = new Vector3(5f, 0f, 0f); // boundary: <= detectRange

            var destination = _brain.ChooseDestination(Home, player, Home);

            Assert.That(_brain.State, Is.EqualTo(ChaseState.Chasing));
            Assert.That(destination, Is.EqualTo(player));
        }

        [Test]
        public void WhileChasing_PlayerBetweenRanges_KeepsChasing()
        {
            // Hysteresis: once chasing, 6 units isn't enough to escape (loseRange is 8).
            _brain.ChooseDestination(Home, new Vector3(4f, 0f, 0f), Home);

            _brain.ChooseDestination(Home, new Vector3(6f, 0f, 0f), Home);

            Assert.That(_brain.State, Is.EqualTo(ChaseState.Chasing));
        }

        [Test]
        public void PlayerEscapesBeyondLoseRange_EnemyReturnsHome()
        {
            var self = new Vector3(1f, 0f, 0f);
            _brain.ChooseDestination(self, new Vector3(4f, 0f, 0f), Home);

            var destination = _brain.ChooseDestination(self, new Vector3(20f, 0f, 0f), Home);

            Assert.That(_brain.State, Is.EqualTo(ChaseState.Returning));
            Assert.That(destination, Is.EqualTo(Home));
        }

        [Test]
        public void HeightDifference_IsIgnored()
        {
            // The player's capsule centre is higher than the ground; only XZ distance counts.
            _brain.ChooseDestination(Home, new Vector3(5f, 3f, 0f), Home);

            Assert.That(_brain.State, Is.EqualTo(ChaseState.Chasing));
        }

        [Test]
        public void Destination_KeepsTheEnemysOwnHeight()
        {
            var self = new Vector3(0f, 1f, 0f);

            var destination = _brain.ChooseDestination(self, new Vector3(2f, 5f, 0f), Home);

            Assert.That(destination.y, Is.EqualTo(1f));
        }

        [Test]
        public void LoseRangeSmallerThanDetectRange_IsRejected()
        {
            Assert.Throws<ArgumentException>(() => new ChaseBrain(detectRange: 5f, loseRange: 4f));
        }
    }
}
