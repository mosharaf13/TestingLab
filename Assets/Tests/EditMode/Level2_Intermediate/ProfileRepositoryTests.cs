using System.Text.RegularExpressions;
using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Builders;
using TestingLab.Tests.Shared.Fakes;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Gameplay/Infrastructure/ProfileRepository.cs
    // In the game:     your progress surviving between scenes and sessions.
    //
    // Topics: a fake storage so tests never touch your real save, the
    //         "round trip" test (save → load → same data), handling bad input,
    //         and LogAssert for expected warnings.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class ProfileRepositoryTests
    {
        private InMemoryKeyValueStorage _storage;
        private ProfileRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _storage = new InMemoryKeyValueStorage();
            _repository = new ProfileRepository(_storage);
        }

        [Test]
        public void Load_WithNothingSaved_ReturnsNull()
        {
            Assert.That(_repository.Load(), Is.Null);
        }

        [Test]
        public void SaveThenLoad_RoundTripsEveryField()
        {
            PlayerProfile original = A.Profile()
                .WithCoins(42).AtLevel(2).WithRunScore(300).WithBestScore(500)
                .WithMaxHealth(150).WithArmor(20).WithMoveSpeed(7.5f);

            _repository.Save(original);
            var loaded = _repository.Load();

            Assert.That(loaded, Is.Not.SameAs(original), "it really came from storage");
            Assert.That(loaded.Wallet.Coins, Is.EqualTo(42));
            Assert.That(loaded.LevelIndex, Is.EqualTo(2));
            Assert.That(loaded.RunScore, Is.EqualTo(300));
            Assert.That(loaded.BestScore, Is.EqualTo(500));
            Assert.That(loaded.Stats.MaxHealth, Is.EqualTo(150));
            Assert.That(loaded.Stats.Armor, Is.EqualTo(20));
            Assert.That(loaded.Stats.MoveSpeed, Is.EqualTo(7.5f).Within(0.0001f));
        }

        [Test]
        public void CorruptedSave_IsIgnoredWithAWarning()
        {
            _storage.SetString(ProfileRepository.DefaultKey, "{\"version\":1,\"maxHealth\":-5,\"moveSpeed\":6}");

            // LogAssert.Expect says "this log message MUST appear". (Unexpected
            // Debug.LogError messages fail a test automatically.)
            LogAssert.Expect(LogType.Warning, new Regex("unreadable save data"));
            var loaded = _repository.Load();

            Assert.That(loaded, Is.Null);
        }

        [Test]
        public void Delete_RemovesTheSave()
        {
            _repository.Save(A.Profile());

            _repository.Delete();

            Assert.That(_storage.HasKey(ProfileRepository.DefaultKey), Is.False);
        }
    }
}
