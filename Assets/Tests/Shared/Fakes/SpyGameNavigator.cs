using System.Collections.Generic;
using TestingLab.Core.Progression;

namespace TestingLab.Tests.Shared.Fakes
{
    // ─────────────────────────────────────────────────────────────────────────
    // A SPY: does nothing except remember how it was called. Instead of really
    // loading scenes, it writes "Arena", "Shop"... into a list for the test to check.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class SpyGameNavigator : IGameNavigator
    {
        public const string MainMenu = nameof(MainMenu);
        public const string Arena = nameof(Arena);
        public const string Shop = nameof(Shop);

        private readonly List<string> _calls = new List<string>();

        public IReadOnlyList<string> Calls => _calls;
        public string LastDestination => _calls.Count > 0 ? _calls[_calls.Count - 1] : null;

        public void GoToMainMenu() => _calls.Add(MainMenu);

        public void GoToArena() => _calls.Add(Arena);

        public void GoToShop() => _calls.Add(Shop);
    }
}
