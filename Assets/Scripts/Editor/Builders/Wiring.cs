using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace TestingLab.EditorTools.Builders
{
    /// <summary>
    /// Assigns [SerializeField] private fields from editor code, exactly like
    /// dragging a reference into the Inspector. Fails loudly on a typo.
    /// </summary>
    internal static class Wiring
    {
        public static void Set(Object target, string fieldName, Object value)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(fieldName);
            if (property == null)
                throw new ArgumentException($"{target.GetType().Name} has no serialized field '{fieldName}'.");

            property.objectReferenceValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        public static void SetString(Object target, string fieldName, string value)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(fieldName)
                ?? throw new ArgumentException($"{target.GetType().Name} has no serialized field '{fieldName}'.");

            property.stringValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
