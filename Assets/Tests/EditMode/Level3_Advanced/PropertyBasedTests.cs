using System.Collections.Generic;
using NUnit.Framework;
using TestingLab.Core.Common;
using TestingLab.Core.Items;
using TestingLab.Core.Scoring;
using TestingLab.Core.Waves;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Topic: PROPERTY-BASED testing (by hand).
    //
    // Example tests check a few inputs YOU thought of. Property tests state a rule
    // that must ALWAYS hold ("an inventory never uses more slots than it has"),
    // then hammer the code with thousands of random operations.
    //
    // The randomness is SEEDED: the same seed gives the same sequence, so a
    // failure is perfectly reproducible. [Values] runs each test once per seed.
    // If one fails, the message includes the seed and step so you can debug it.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced)]
    public class PropertyBasedTests
    {
        private const int Steps = 2000;
        private static readonly string[] ItemIds = { "coin", "gem", "potion", "arrow" };

        [Test]
        public void Inventory_NeverExceedsCapacity_AndCountsNeverGoNegative([Values(1, 2, 3, 42, 1337)] int seed)
        {
            var random = new SystemRandomSource(seed);
            var inventory = new Inventory(slotCapacity: 4, maxStack: 5);

            for (int step = 0; step < Steps; step++)
            {
                string id = ItemIds[random.Index(ItemIds.Length)];
                int count = 1 + random.Index(7);

                if (random.Value01() < 0.6f)
                    inventory.Add(id, count);
                else
                    inventory.Remove(id, count);

                Assert.That(inventory.UsedSlots, Is.LessThanOrEqualTo(inventory.SlotCapacity), $"seed {seed}, step {step}");
                foreach (var pair in inventory.Items)
                    Assert.That(pair.Value, Is.GreaterThan(0), $"seed {seed}, step {step}: {pair.Key} has a zero/negative count");
            }
        }

        [Test]
        public void Wallet_BalanceAlwaysMatchesASimpleModel([Values(7, 99, 2024)] int seed)
        {
            // "Model-based" testing: compare the real class with a dead-simple model.
            var random = new SystemRandomSource(seed);
            var wallet = new Wallet();
            int model = 0;

            for (int step = 0; step < Steps; step++)
            {
                int amount = random.Index(20);
                if (random.Value01() < 0.5f)
                {
                    wallet.Earn(amount);
                    model += amount;
                }
                else if (wallet.TrySpend(amount))
                {
                    model -= amount;
                }

                Assert.That(wallet.Coins, Is.EqualTo(model), $"seed {seed}, step {step}");
                Assert.That(wallet.Coins, Is.GreaterThanOrEqualTo(0));
            }
        }

        [Test]
        public void ScoreKeeper_MultiplierStaysInRange_AndScoreNeverDecreases([Values(5, 50, 500)] int seed)
        {
            var random = new SystemRandomSource(seed);
            var clock = new FakeTimeProvider();
            var score = new ScoreKeeper(clock);
            int previousScore = 0;

            for (int step = 0; step < Steps; step++)
            {
                clock.Advance(random.Range(0f, 4f));
                score.AddPoints(random.Index(50));

                Assert.That(score.Multiplier, Is.InRange(1, ScoreKeeper.MaxMultiplier), $"seed {seed}, step {step}");
                Assert.That(score.Score, Is.GreaterThanOrEqualTo(previousScore), $"seed {seed}, step {step}");
                previousScore = score.Score;
            }
        }

        [Test]
        public void WaveSchedule_StartsEveryWaveExactlyOnce_WhateverTheFrameRate([Values(11, 22, 33)] int seed)
        {
            // Frames can be 1 ms or 500 ms long. The schedule must not care.
            var random = new SystemRandomSource(seed);
            var waves = new List<WaveDefinition>();
            for (int i = 0; i < 20; i++)
                waves.Add(new WaveDefinition(random.Range(0f, 60f), 1, 0));

            var schedule = new WaveSchedule(waves);
            var started = new List<int>();
            schedule.WaveStarted += (_, index) => started.Add(index);

            while (schedule.Elapsed < 61f)
                schedule.Advance(random.Range(0.001f, 0.5f));

            Assert.That(started, Is.EqualTo(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }), $"seed {seed}");
        }
    }
}
