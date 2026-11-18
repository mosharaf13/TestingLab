using UnityEngine;

namespace TestingLab.Gameplay.UI
{
    /// <summary>Shared colours and sizes, so every screen looks like the same game.</summary>
    public static class UiTheme
    {
        public static readonly Color Background = new Color(0.08f, 0.10f, 0.16f, 0.92f);
        public static readonly Color Panel = new Color(0.13f, 0.16f, 0.24f, 0.96f);
        public static readonly Color Button = new Color(0.20f, 0.45f, 0.85f, 1f);
        public static readonly Color ButtonHighlight = new Color(0.30f, 0.58f, 1f, 1f);
        public static readonly Color ButtonPressed = new Color(0.14f, 0.33f, 0.66f, 1f);
        public static readonly Color DangerButton = new Color(0.75f, 0.25f, 0.25f, 1f);
        public static readonly Color Text = new Color(0.95f, 0.96f, 1f, 1f);
        public static readonly Color MutedText = new Color(0.70f, 0.75f, 0.85f, 1f);
        public static readonly Color Accent = new Color(1f, 0.80f, 0.15f, 1f);
        public static readonly Color HealthFill = new Color(0.30f, 0.85f, 0.40f, 1f);
        public static readonly Color HealthBack = new Color(0.25f, 0.08f, 0.08f, 0.9f);

        public const int TitleSize = 64;
        public const int HeadingSize = 36;
        public const int BodySize = 26;
        public const int SmallSize = 20;
    }
}
