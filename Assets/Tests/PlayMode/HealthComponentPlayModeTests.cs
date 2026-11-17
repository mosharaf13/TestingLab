using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode
{
    // In Play Mode, Awake DOES run when you AddComponent, unlike in Edit Mode.
    public class HealthComponentPlayModeTests
    {
        [UnityTest]
        public IEnumerator Awake_InitialisesHealthFromInspectorValue()
        {
            var go = new GameObject("Enemy");
            var component = go.AddComponent<HealthComponent>();
            yield return null;

            Assert.That(component.Health, Is.Not.Null);
            Assert.That(component.Health.Max, Is.EqualTo(100));

            Object.Destroy(go);
        }

        [UnityTest]
        public IEnumerator Death_DestroysTheGameObject()
        {
            var go = new GameObject("Enemy");
            var component = go.AddComponent<HealthComponent>();

            LogAssert.Expect(LogType.Log, "Enemy died");
            component.Health.TakeDamage(1000);
            yield return null;

            Assert.That(go == null, Is.True);
        }
    }
}
