using TestingLab.Gameplay.Player;
using UnityEngine;

namespace TestingLab.Tests.Shared.Fakes
{
    /// <summary>A STUB input: the test sets Move and the player obeys. No keyboard needed.</summary>
    public sealed class ScriptedPlayerInput : IPlayerInput
    {
        public Vector2 Move { get; set; }
    }
}
