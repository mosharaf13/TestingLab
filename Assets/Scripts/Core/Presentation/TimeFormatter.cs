using System;

namespace TestingLab.Core.Presentation
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER  (your first [TestCase] table)
    // Tested by:      Tests/EditMode/Level1_Beginner/TimeFormatterTests.cs
    //
    // Turns seconds into the "m:ss" text shown on the HUD timer.
    // Rounds UP, so the timer reads 0:01 until time is really over (players feel
    // cheated if it shows 0:00 while they're still alive).
    // ─────────────────────────────────────────────────────────────────────────

    public static class TimeFormatter
    {
        public static string MinutesSeconds(float seconds)
        {
            if (float.IsNaN(seconds) || seconds <= 0f)
                return "0:00";

            int whole = (int)Math.Ceiling(seconds);
            return $"{whole / 60}:{whole % 60:00}";
        }
    }
}
