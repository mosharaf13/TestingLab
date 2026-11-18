using System.Collections;
using NUnit.Framework;
using TestingLab.Tests.Shared;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Exercises
{
    // ═════════════════════════════════════════════════════════════════════════
    //  PLAY MODE EXERCISES
    //  Remove [Ignore], replace Assert.Fail, and make each test green.
    //  Look at the matching example test named in each hint.
    // ═════════════════════════════════════════════════════════════════════════

    [Category(TestCategory.Intermediate), Category(TestCategory.Exercise)]
    public class IntermediatePlayModeExercises
    {
        [UnityTest, Ignore("Exercise P1 (see MoverTests): calling Mover.Stop() mid-move stops it; it never raises Arrived")]
        public IEnumerator Mover_StopMidMove_NeverArrives()
        {
            Assert.Fail("TODO");
            yield break;
        }

        [UnityTest, Ignore("Exercise P2 (see MoverTests): MoveTo a new target while moving goes to the NEW target")]
        public IEnumerator Mover_ChangeTargetMidMove()
        {
            Assert.Fail("TODO");
            yield break;
        }

        [UnityTest, Ignore("Exercise P3 (see ContactDamageTests): with GodMode on, touching an enemy deals no damage")]
        public IEnumerator ContactDamage_GodMode_NoDamage()
        {
            Assert.Fail("TODO");
            yield break;
        }
    }

    [Category(TestCategory.Advanced), Category(TestCategory.Exercise)]
    public class AdvancedPlayModeExercises
    {
        [UnityTest, Ignore("Exercise P4 (see ArenaSceneTests): running out of time shows \"Time's Up!\" (hint: Time.timeScale = 20)")]
        public IEnumerator Arena_TimeUp_ShowsTimesUp()
        {
            Assert.Fail("TODO");
            yield break;
        }

        [UnityTest, Ignore("Exercise P5 (see ShopSceneTests): AFTER fixing ShopCart, buying two Heart Containers gives +50 max health")]
        public IEnumerator Shop_BuyTwoOfTheSameItem()
        {
            Assert.Fail("TODO");
            yield break;
        }

        [UnityTest, Ignore("Exercise P6 (see MainMenuSceneTests): after winning the last level, the menu says you cleared every level")]
        public IEnumerator MainMenu_AfterFinishingTheRun_ShowsMessage()
        {
            Assert.Fail("TODO");
            yield break;
        }
    }
}
