using NUnit.Framework;
using TestingLab.Core.Shop;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // Learning level: BEGINNER
    // Code under test: Assets/Scripts/Core/Shop/CouponBook.cs
    // In the game: the coupon box in the Shop.
    // Topic: "out" parameters and testing both the return value AND the output.

    [Category(TestCategory.Beginner)]
    public class CouponBookTests
    {
        [Test]
        public void KnownCode_ReturnsTrueAndThePercent()
        {
            var book = new CouponBook();
            book.Register("SAVE10", 10);

            bool found = book.TryGetPercent("SAVE10", out int percent);

            Assert.That(found, Is.True);
            Assert.That(percent, Is.EqualTo(10));
        }

        [TestCase("save10")]
        [TestCase("  SAVE10  ")]
        public void Codes_IgnoreCaseAndSpaces(string typed)
        {
            var book = new CouponBook();
            book.Register("SAVE10", 10);

            Assert.That(book.TryGetPercent(typed, out _), Is.True);
        }

        [TestCase("NOPE")]
        [TestCase("")]
        [TestCase((string)null)]   // the cast matters: a bare null confuses NUnit
        public void UnknownOrEmptyCode_ReturnsFalse(string typed)
        {
            var book = CouponBook.CreateDefault();

            bool found = book.TryGetPercent(typed, out int percent);

            Assert.That(found, Is.False);
            Assert.That(percent, Is.EqualTo(0));
        }
    }
}
