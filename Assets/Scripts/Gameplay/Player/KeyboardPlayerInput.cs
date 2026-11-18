using UnityEngine;

namespace TestingLab.Gameplay.Player
{
    /// <summary>
    /// WASD / arrow keys. Deliberately untested: it only forwards what the Input
    /// Manager says. That's the "humble object" — too simple to break.
    /// </summary>
    public sealed class KeyboardPlayerInput : IPlayerInput
    {
        public Vector2 Move
        {
            get
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
                return Vector2.zero;
#endif
            }
        }
    }
}
