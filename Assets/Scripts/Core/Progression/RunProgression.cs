using System;

namespace TestingLab.Core.Progression
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED  (mocks & spies: testing INTERACTIONS)
    // Tested by:      Tests/EditMode/Level3_Advanced/RunProgressionTests.cs
    // Pattern:        Application service / use case, constructor injection
    //
    // The "game flow" of a whole run across scenes:
    //   Main Menu → Arena (level 1) → Shop → Arena (level 2) → … → Main Menu
    //
    // Its job is mostly to tell OTHER objects what to do (save the profile, change
    // scene). There's little state to assert on, so its tests check that the
    // right calls were made on test doubles: a spy navigator and a fake store.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class RunProgression
    {
        private readonly IProfileStore _store;
        private readonly IGameNavigator _navigator;

        public int LevelCount { get; }
        public PlayerProfile Profile { get; private set; }

        public bool IsRunComplete => Profile.LevelIndex >= LevelCount;
        public bool HasRunInProgress => Profile.LevelIndex > 0 && !IsRunComplete;

        public RunProgression(IProfileStore store, IGameNavigator navigator, int levelCount)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            if (levelCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(levelCount));

            LevelCount = levelCount;
            Profile = _store.Load() ?? PlayerProfile.NewRun();
        }

        public void StartNewRun()
        {
            Profile = PlayerProfile.NewRun(Profile.BestScore);
            _store.Save(Profile);
            _navigator.GoToArena();
        }

        public void ContinueRun()
        {
            if (!HasRunInProgress)
                throw new InvalidOperationException("There is no run in progress to continue.");

            _navigator.GoToShop();
        }

        public void RecordLevelResult(bool won, int coinsCollected, int levelScore)
        {
            Profile.CompleteLevel(won, coinsCollected, levelScore);
            _store.Save(Profile);
        }

        /// <summary>After winning: shop if more levels remain, otherwise back to the menu.</summary>
        public void ContinueAfterWin()
        {
            if (IsRunComplete)
                _navigator.GoToMainMenu();
            else
                _navigator.GoToShop();
        }

        public void RetryLevel() => _navigator.GoToArena();

        /// <summary>Saves without changing scene (e.g. right after buying something).</summary>
        public void SaveProgress() => _store.Save(Profile);

        public void LeaveShop()
        {
            _store.Save(Profile);
            _navigator.GoToArena();
        }

        public void QuitToMenu() => _navigator.GoToMainMenu();
    }
}
