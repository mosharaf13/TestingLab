using System;
using System.Collections;
using UnityEngine;

namespace TestingLab.Tests.Shared.Support
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE → ADVANCED (Play Mode)
    //
    //   yield return WaitFor.Condition(() => enemy == null, 2f, "enemy never died");
    //
    // RULE: never write an open-ended "while (!x) yield return null;" in a test.
    // If x never happens, the test hangs the whole Test Runner. Always time out.
    //
    // Uses REAL time (realtimeSinceStartup) so it still works when the game has
    // set Time.timeScale = 0 (paused / level over).
    // ─────────────────────────────────────────────────────────────────────────

    public static class WaitFor
    {
        public static IEnumerator Condition(Func<bool> condition, float timeoutSeconds, string failureMessage)
        {
            float giveUpAt = Time.realtimeSinceStartup + timeoutSeconds;
            while (!condition())
            {
                if (Time.realtimeSinceStartup > giveUpAt)
                    throw new TimeoutException($"Timed out after {timeoutSeconds}s: {failureMessage}");
                yield return null;
            }
        }

        public static IEnumerator Frames(int count)
        {
            for (int i = 0; i < count; i++)
                yield return null;
        }

        /// <summary>
        /// Lets two physics steps run (triggers fire during the physics step), then one
        /// more frame so any Destroy() calls have landed.
        ///
        /// TRAP: physics does not step while Time.timeScale is 0. Waiting for
        /// WaitForFixedUpdate then hangs FOREVER. In the Arena, collecting the winning
        /// coin pauses the game (timeScale = 0) in the middle of this wait, so we
        /// check before every step and stop waiting once physics is frozen.
        /// </summary>
        public static IEnumerator Physics()
        {
            for (int step = 0; step < 2; step++)
            {
                if (Time.timeScale <= 0f)
                    break;
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
    }
}
