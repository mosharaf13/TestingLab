using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Builders;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Progression/PlayerProfile.cs
    // Topic: the Test Data Builder (A.Profile()) keeps each test focused on the
    //        one or two values it cares about.

    [Category(TestCategory.Intermediate)]
    public class PlayerProfileTests
    {
        [Test]
        public void WinningALevel_BanksCoinsAndScore_AndMovesOn()
        {
            PlayerProfile profile = A.Profile().WithCoins(5).AtLevel(0);

            profile.CompleteLevel(won: true, coinsCollected: 10, levelScore: 200);

            Assert.That(profile.Wallet.Coins, Is.EqualTo(15));
            Assert.That(profile.RunScore, Is.EqualTo(200));
            Assert.That(profile.LevelIndex, Is.EqualTo(1));
        }

        [Test]
        public void LosingALevel_KeepsYouOnTheSameLevel_WithNoReward()
        {
            PlayerProfile profile = A.Profile().WithCoins(5).AtLevel(1).WithRunScore(100);

            profile.CompleteLevel(won: false, coinsCollected: 10, levelScore: 200);

            Assert.That(profile.Wallet.Coins, Is.EqualTo(5));
            Assert.That(profile.RunScore, Is.EqualTo(100));
            Assert.That(profile.LevelIndex, Is.EqualTo(1));
        }

        [Test]
        public void GreatLosingAttempt_CanStillSetBestScore()
        {
            PlayerProfile profile = A.Profile().WithRunScore(100).WithBestScore(150);

            profile.CompleteLevel(won: false, coinsCollected: 0, levelScore: 80);

            Assert.That(profile.BestScore, Is.EqualTo(180));
        }

        [Test]
        public void NewRun_KeepsOnlyTheBestScore()
        {
            var profile = PlayerProfile.NewRun(bestScore: 999);

            Assert.That(profile.BestScore, Is.EqualTo(999));
            Assert.That(profile.LevelIndex, Is.EqualTo(0));
            Assert.That(profile.Wallet.Coins, Is.EqualTo(0));
            Assert.That(profile.Stats.MaxHealth, Is.EqualTo(PlayerStats.DefaultMaxHealth));
        }
    }
}
