using NUnit.Framework;
using TestingLab.Core.Common;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level3_Advanced.Contracts
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Topic: CONTRACT tests (abstract test fixtures).
    //
    // Every IKeyValueStorage must behave the same way, or swapping one for another
    // would break the game. This ABSTRACT class holds the tests ONCE. Each
    // implementation gets a tiny subclass that only says how to create it.
    // NUnit runs every test below once per subclass.
    //
    // This is also how we know our fake (InMemoryKeyValueStorage) is trustworthy:
    // it passes the same contract as the real PlayerPrefsStorage. A fake that
    // behaves differently from the real thing makes tests lie.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced)]
    public abstract class KeyValueStorageContract
    {
        protected const string Key = "TestingLab.ContractTests.Key";

        protected IKeyValueStorage Storage { get; private set; }

        protected abstract IKeyValueStorage CreateStorage();

        [SetUp]
        public void CreateAndClean()
        {
            Storage = CreateStorage();
            Storage.DeleteKey(Key);
        }

        [TearDown]
        public void Clean() => Storage.DeleteKey(Key);

        [Test]
        public void MissingKey_HasKeyIsFalse_AndGetReturnsNull()
        {
            Assert.That(Storage.HasKey(Key), Is.False);
            Assert.That(Storage.GetString(Key), Is.Null);
        }

        [Test]
        public void SetThenGet_ReturnsTheValue()
        {
            Storage.SetString(Key, "hello");

            Assert.That(Storage.HasKey(Key), Is.True);
            Assert.That(Storage.GetString(Key), Is.EqualTo("hello"));
        }

        [Test]
        public void SetTwice_LastValueWins()
        {
            Storage.SetString(Key, "first");
            Storage.SetString(Key, "second");

            Assert.That(Storage.GetString(Key), Is.EqualTo("second"));
        }

        [Test]
        public void Delete_RemovesTheKey()
        {
            Storage.SetString(Key, "value");

            Storage.DeleteKey(Key);

            Assert.That(Storage.HasKey(Key), Is.False);
        }

        [Test]
        public void DeletingAMissingKey_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Storage.DeleteKey(Key));
        }
    }
}
