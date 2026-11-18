using System.Collections.Generic;
using TestingLab.Core.Common;

namespace TestingLab.Tests.Shared.Fakes
{
    // ─────────────────────────────────────────────────────────────────────────
    // A STUB: returns canned answers. The test decides what the "dice" roll.
    //
    //   var random = new FakeRandomSource { NextValue = 0.5f };   // always 0.5
    //   random.Enqueue(0.1f, 0.9f);                               // then 0.1, 0.9, then 0.5...
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class FakeRandomSource : IRandomSource
    {
        private readonly Queue<float> _queued = new Queue<float>();

        /// <summary>Returned whenever no queued values are left.</summary>
        public float NextValue { get; set; }

        /// <summary>How many times the code under test asked for a number.</summary>
        public int CallCount { get; private set; }

        public FakeRandomSource Enqueue(params float[] values)
        {
            foreach (float value in values)
                _queued.Enqueue(value);
            return this;
        }

        public float Value01()
        {
            CallCount++;
            return _queued.Count > 0 ? _queued.Dequeue() : NextValue;
        }
    }
}
