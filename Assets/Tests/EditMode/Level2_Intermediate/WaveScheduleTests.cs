using System.Collections.Generic;
using NUnit.Framework;
using TestingLab.Core.Waves;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Waves/WaveSchedule.cs
    // In the game:     when new coins and enemies appear in the Arena.
    //
    // Topic: simulating time by calling Advance() like Update() would, and
    //        capturing events in a list to assert on what happened and when.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class WaveScheduleTests
    {
        private List<int> _startedIndexes;

        private WaveSchedule CreateSchedule(params WaveDefinition[] waves)
        {
            var schedule = new WaveSchedule(waves);
            _startedIndexes = new List<int>();
            schedule.WaveStarted += (wave, index) => _startedIndexes.Add(index);
            return schedule;
        }

        [Test]
        public void WaveAtTimeZero_StartsOnFirstAdvance()
        {
            var schedule = CreateSchedule(new WaveDefinition(0f, 3, 0));

            schedule.Advance(0f);

            Assert.That(_startedIndexes, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void Wave_StartsWhenItsTimeArrives_NotBefore()
        {
            var schedule = CreateSchedule(new WaveDefinition(5f, 3, 0));

            schedule.Advance(4.9f);
            Assert.That(_startedIndexes, Is.Empty);

            schedule.Advance(0.1f);
            Assert.That(_startedIndexes, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void BigTimeJump_StartsEveryOverdueWave_InOrder()
        {
            // A long frame (e.g. a lag spike) must not skip waves.
            var schedule = CreateSchedule(
                new WaveDefinition(1f, 1, 0),
                new WaveDefinition(2f, 1, 0),
                new WaveDefinition(3f, 1, 0));

            schedule.Advance(10f);

            Assert.That(_startedIndexes, Is.EqualTo(new[] { 0, 1, 2 }));
            Assert.That(schedule.IsFinished, Is.True);
        }

        [Test]
        public void Waves_GivenOutOfOrder_StartSortedByTime()
        {
            var late = new WaveDefinition(9f, 1, 0);
            var early = new WaveDefinition(1f, 2, 0);
            var started = new List<float>();
            var schedule = new WaveSchedule(new[] { late, early });
            schedule.WaveStarted += (wave, _) => started.Add(wave.StartTime);

            schedule.Advance(10f);

            Assert.That(started, Is.EqualTo(new[] { 1f, 9f }));
        }

        [Test]
        public void EachWave_StartsOnlyOnce()
        {
            var schedule = CreateSchedule(new WaveDefinition(0f, 1, 0));

            schedule.Advance(1f);
            schedule.Advance(1f);
            schedule.Advance(1f);

            Assert.That(_startedIndexes.Count, Is.EqualTo(1));
        }

        [Test]
        public void TotalCoins_SumsAllWaves()
        {
            var schedule = CreateSchedule(new WaveDefinition(0f, 3, 0), new WaveDefinition(5f, 4, 2));

            Assert.That(schedule.TotalCoins, Is.EqualTo(7));
        }
    }
}
