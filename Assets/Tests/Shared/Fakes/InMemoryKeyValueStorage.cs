using System.Collections.Generic;
using TestingLab.Core.Common;

namespace TestingLab.Tests.Shared.Fakes
{
    /// <summary>A FAKE save system: a dictionary. Every test gets a clean, private one.</summary>
    public sealed class InMemoryKeyValueStorage : IKeyValueStorage
    {
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        public int Count => _values.Count;

        public bool HasKey(string key) => _values.ContainsKey(key);

        public string GetString(string key) => _values.TryGetValue(key, out var value) ? value : null;

        public void SetString(string key, string value) => _values[key] = value;

        public void DeleteKey(string key) => _values.Remove(key);
    }
}
