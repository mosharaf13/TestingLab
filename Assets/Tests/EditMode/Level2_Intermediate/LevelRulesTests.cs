using NUnit.Framework;
using TestingLab.Core.Waves;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Waves/LevelRules.cs
    // These test the RULES. Level3_Advanced/LevelAssetValidationTests then applies
    // the same rules to every real level asset in the project.

    [Category(TestCategory.Intermediate)]
    public class LevelRulesTests
    {
        [Test]
        public void SensibleLevel_HasNoErrors()
        {
            var waves = new[] { new WaveDefinition(0f, 5, 0), new WaveDefinition(10f, 5, 1) };

            var errors = LevelRules.Validate(timeLimitSeconds: 60f, coinGoal: 10, waves);

            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void GoalHigherThanCoinsSpawned_IsReported()
        {
            var waves = new[] { new WaveDefinition(0f, 5, 0) };

            var errors = LevelRules.Validate(60f, coinGoal: 6, waves);

            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Does.Contain("never be won"));
        }

        [Test]
        public void WaveAfterTimeLimit_IsReported()
        {
            var waves = new[] { new WaveDefinition(0f, 10, 0), new WaveDefinition(60f, 1, 0) };

            var errors = LevelRules.Validate(60f, 10, waves);

            Assert.That(errors, Has.Some.Contains("time limit"));
        }

        [Test]
        public void NoWaves_IsReported()
        {
            var errors = LevelRules.Validate(60f, 10, new WaveDefinition[0]);

            Assert.That(errors, Has.Some.Contains("at least one wave"));
        }
    }
}
