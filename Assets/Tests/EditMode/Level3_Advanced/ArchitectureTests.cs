using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TestingLab.Core.Combat;
using TestingLab.Gameplay.Flow;
using TestingLab.Tests.Shared;
using UnityEngine;

namespace TestingLab.Tests.EditMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Topic: ARCHITECTURE tests — tests about the CODE ITSELF, using reflection.
    //
    // Team rules ("Core must not use Unity", "every test has a level category")
    // are easy to agree on and easy to forget. Writing them as tests means the
    // computer reminds whoever breaks them, every time.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced)]
    public class ArchitectureTests
    {
        private static readonly Assembly CoreAssembly = typeof(Health).Assembly;
        private static readonly Assembly GameplayAssembly = typeof(ArenaController).Assembly;

        [Test]
        public void Core_DoesNotReferenceUnity()
        {
            // Core's asmdef has "No Engine References" ticked. This test keeps it that
            // way: pure C# can be tested anywhere, instantly, with no scene.
            var unityReferences = CoreAssembly.GetReferencedAssemblies()
                .Select(a => a.Name)
                .Where(name => name.StartsWith("UnityEngine") || name.StartsWith("UnityEditor"))
                .ToList();

            Assert.That(unityReferences, Is.Empty);
        }

        [Test]
        public void MonoBehaviours_HaveNoPublicFields()
        {
            // Convention: [SerializeField] private for the Inspector, properties for code.
            // Public fields let any class change anything, which makes bugs hard to trace.
            var offenders = GameplayAssembly.GetTypes()
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t))
                .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(f => $"{t.Name}.{f.Name}"))
                .ToList();

            Assert.That(offenders, Is.Empty);
        }

        [Test]
        public void EveryTestClass_HasALearningLevelCategory()
        {
            string[] levels = { TestCategory.Beginner, TestCategory.Intermediate, TestCategory.Advanced };

            var testAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith("TestingLab.Tests."));

            var uncategorised = testAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && HasTests(t))
                .Where(t => !CategoriesOf(t).Intersect(levels).Any())
                .Select(t => t.FullName)
                .ToList();

            Assert.That(uncategorised, Is.Empty,
                "Add [Category(TestCategory.Beginner/Intermediate/Advanced)] to these classes");
        }

        [Test]
        public void TestNamespaces_MatchTheirCategory()
        {
            // A class in a Level1_Beginner folder should be tagged Beginner, etc.
            var mismatches = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith("TestingLab.Tests."))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && HasTests(t) && t.Namespace != null)
                .Where(t => (t.Namespace.Contains("Level1_Beginner") && !CategoriesOf(t).Contains(TestCategory.Beginner))
                         || (t.Namespace.Contains("Level2_Intermediate") && !CategoriesOf(t).Contains(TestCategory.Intermediate))
                         || (t.Namespace.Contains("Level3_Advanced") && !CategoriesOf(t).Contains(TestCategory.Advanced)))
                .Select(t => t.FullName)
                .ToList();

            Assert.That(mismatches, Is.Empty);
        }

        private static bool HasTests(Type type) =>
            type.GetMethods().Any(m => m.GetCustomAttributes(inherit: true)
                .Any(a => a is TestAttribute || a is TestCaseAttribute || a is TestCaseSourceAttribute
                          || a.GetType().Name == "UnityTestAttribute"));

        private static string[] CategoriesOf(Type type) =>
            type.GetCustomAttributes<CategoryAttribute>(inherit: true).Select(c => c.Name).ToArray();
    }
}
