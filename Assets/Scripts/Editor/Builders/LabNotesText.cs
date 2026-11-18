namespace TestingLab.EditorTools.Builders
{
    /// <summary>The F1 overlay text for each scene: "what you see" → "which test covers it".</summary>
    internal static class LabNotesText
    {
        public const string MainMenu =
@"<b>MAIN MENU</b>  — what protects this screen?

<b>Best score / Continue button</b>
  Data comes from RunProgression + PlayerProfile (pure C#).
  → EditMode ▸ Level3_Advanced ▸ RunProgressionTests  (mocks & spies)
  → EditMode ▸ Level2_Intermediate ▸ ProfileRepositoryTests  (fake storage)

<b>Clicking Play loads the Arena</b>
  Needs real scenes and real UI → a Play Mode UI test.
  → PlayMode ▸ Level3_Advanced ▸ MainMenuSceneTests

<b>Is this scene set up correctly?</b> (no missing scripts, refs assigned)
  → EditMode ▸ Level3_Advanced ▸ SceneSetupValidationTests

Rule of thumb: test LOGIC in Edit Mode (fast), test WIRING in Play Mode (slow, few).";

        public const string Arena =
@"<b>ARENA</b>  — every system on screen and its test

<b>Health bar, damage, armor</b>   Health, DamageCalculator
  → EditMode ▸ Level1_Beginner ▸ HealthTests
  → EditMode ▸ Level2_Intermediate ▸ DamageCalculatorTests (fake random)
  → PlayMode ▸ Level2_Intermediate ▸ ContactDamageTests (real triggers)

<b>Score & combo x2..x4</b>   ScoreKeeper
  → EditMode ▸ Level2_Intermediate ▸ ScoreKeeperTests (fake clock)

<b>Timer, coin goal, win/lose, pause</b>   GameSession
  → EditMode ▸ Level2_Intermediate ▸ GameSessionTests (state machine)
  → EditMode ▸ Level1_Beginner ▸ TimeFormatterTests

<b>Waves of coins & enemies</b>   WaveSchedule, SpawnPointPicker
  → EditMode ▸ Level2_Intermediate ▸ WaveScheduleTests, SpawnPointPickerTests

<b>Enemy chasing</b>   ChaseBrain (decisions) + Mover (movement)
  → EditMode ▸ Level2_Intermediate ▸ ChaseBrainTests
  → PlayMode ▸ Level2_Intermediate ▸ MoverTests

<b>Picking up coins</b>   Collector + Collectible (physics triggers)
  → PlayMode ▸ Level2_Intermediate ▸ CollectibleTests

<b>Player movement</b>   MovementMath + PlayerController
  → EditMode ▸ Level2_Intermediate ▸ MovementMathTests
  → PlayMode ▸ Level2_Intermediate ▸ PlayerControllerTests (scripted input)

<b>The whole thing wired together</b>
  → PlayMode ▸ Level3_Advanced ▸ ArenaSceneTests

<b>Are the level numbers even winnable?</b>
  → EditMode ▸ Level3_Advanced ▸ LevelAssetValidationTests";

        public const string Shop =
@"<b>SHOP</b>  — ⚠ contains deliberate bugs!

Try it: add the same item twice. Look at the cart.
Try it: coupon code MEGA150. Look at the total, then Checkout.

These are the ShopCart bug-hunt bugs. Reproduce each one as a
failing unit test BEFORE fixing it:
  → EditMode ▸ Exercises ▸ Level2_Intermediate ▸ ShopCartBugHuntTests

<b>Checkout rules</b> (money, effects, nothing changes on failure)
  → EditMode ▸ Level2_Intermediate ▸ ShopServiceTests
<b>Item effects</b> (Strategy pattern)
  → EditMode ▸ Level2_Intermediate ▸ ItemEffectTests
<b>Coupons</b>
  → EditMode ▸ Level1_Beginner ▸ CouponBookTests
<b>Buying through the real UI</b>
  → PlayMode ▸ Level3_Advanced ▸ ShopSceneTests";
    }
}
