using TestingLab.Core.Common;
using UnityEngine;

namespace TestingLab.Gameplay.Infrastructure
{
    /// <summary>The real clock. Uses scaled game time, so it stops while paused.</summary>
    public sealed class UnityTimeProvider : ITimeProvider
    {
        public float Now => Time.time;
    }
}
