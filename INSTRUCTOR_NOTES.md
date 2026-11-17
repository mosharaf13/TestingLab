# Instructor notes (don't share with students before the bug hunt)

## ShopCart deliberate bugs

| # | Spec rule | What actually happens | Fix |
|---|-----------|-----------------------|-----|
| 1 | Adding an existing SKU increases its quantity | The quantity is **overwritten** | In `AddItem`, add to the existing quantity if the SKU is present |
| 2 | Removing an unknown SKU returns false | Throws **KeyNotFoundException** | Use `TryGetValue` / `_lines.Remove(sku)` and return its result |
| 3 | Coupon above 100 throws | Accepted, so **TotalCents goes negative** | Also throw when `percent > 100` |

Bug 3 is found twice: by `ApplyCoupon_Over100_Throws` and by `Total_IsNeverNegative`
(applying, say, 150%). Good moment to discuss one bug causing multiple test failures.

**Discussion point, not a bug:** `TotalCents` uses integer division, so a 15% coupon on 999
cents gives a discount of 149 (not 149.85), rounding in the customer's favour. The spec doesn't
say how to round. Ask students whether a test should pin this down, and who decides the answer
(it's a requirements question, not a code question).

## Sample solutions

```csharp
[Test]
public void AddItem_SameSkuTwice_QuantitiesAreSummed()
{
    _cart.AddItem("potion", 200, 2);
    _cart.AddItem("potion", 200, 3);
    Assert.That(_cart.ItemCount, Is.EqualTo(5));          // fails: 3 (bug 1)
}

[Test]
public void RemoveItem_UnknownSku_ReturnsFalse()
{
    Assert.That(_cart.RemoveItem("nothing"), Is.False);   // fails: exception (bug 2)
}

[Test]
public void ApplyCoupon_Over100_Throws()
{
    Assert.Throws<ArgumentOutOfRangeException>(() => _cart.ApplyCoupon(101)); // fails (bug 3)
}

[Test]
public void ApplyCoupon_25Percent_ReducesTotal()
{
    _cart.AddItem("shield", 2000);
    _cart.ApplyCoupon(25);
    Assert.That(_cart.TotalCents, Is.EqualTo(1500));      // passes
}

[Test]
public void Total_IsNeverNegative()
{
    _cart.AddItem("shield", 2000);
    _cart.ApplyCoupon(150);   // once bug 3 is fixed this throws; wrap or rethink the test
    Assert.That(_cart.TotalCents, Is.GreaterThanOrEqualTo(0)); // fails (bug 3)
}
```

Note the last one: after fixing bug 3, `ApplyCoupon(150)` throws, so the test errors instead
of failing on its assertion. That's a nice prompt for "tests change when the design changes."

Fixed `ShopCart` methods:

```csharp
public void AddItem(string sku, int priceCents, int quantity = 1)
{
    // ...existing validation...
    int existing = _lines.TryGetValue(sku, out var line) ? line.quantity : 0;
    _lines[sku] = (priceCents, existing + quantity);
}

public bool RemoveItem(string sku) => sku != null && _lines.Remove(sku);

public void ApplyCoupon(int percent)
{
    if (percent < 0 || percent > 100)
        throw new ArgumentOutOfRangeException(nameof(percent));
    CouponPercent = percent;
}
```

## MoreExercises answers (brief)

- `Inventory_RemoveMoreThanHeld_ReturnsFalse`: add 2, remove 5 → false, count still 2.
- `Health_HealAtFullHealth_DoesNotRaiseChanged`: subscribe a counter, Heal(10) at full → 0 calls.
- `ScoreKeeper_NegativePoints_Throws`: `Assert.Throws<ArgumentOutOfRangeException>`.
- `DamageCalculator_CritWithArmor`: fake roll 0.0, base 100, armor 100 → 50 × 2 = 100.
- `DamageCalculator_InvalidCritChance_Throws`: `[TestCase(1.5f)] [TestCase(-0.1f)]` on the constructor.

## Ideas for extension

- Ask students to add a feature test-first (TDD): e.g. `Inventory.MoveTo(otherInventory, id, count)`.
- Break a passing test on purpose (change `<=` to `<` in ScoreKeeper) and see which test catches it.
  This is mutation testing by hand.
- Have students write a Play Mode test for `Mover` changing target mid-move.
