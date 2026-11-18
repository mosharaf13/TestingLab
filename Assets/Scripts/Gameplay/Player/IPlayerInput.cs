using UnityEngine;

namespace TestingLab.Gameplay.Player
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Pattern:        Strategy / Dependency Inversion for input
    //
    // PlayerController doesn't read the keyboard; it asks an IPlayerInput.
    // The game plugs in KeyboardPlayerInput. Play Mode tests plug in
    // ScriptedPlayerInput (Tests/Shared) and "press" directions from code.
    // ─────────────────────────────────────────────────────────────────────────

    public interface IPlayerInput
    {
        /// <summary>Desired direction: x = right, y = forward. Length 0..1.</summary>
        Vector2 Move { get; }
    }
}
