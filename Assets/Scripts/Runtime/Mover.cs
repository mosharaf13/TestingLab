using System;
using UnityEngine;

namespace TestingLab
{
    /// <summary>
    /// Moves toward a target at a constant speed, one step per frame.
    /// </summary>
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        public Vector3? Target { get; private set; }
        public bool IsMoving => Target.HasValue;
        public event Action Arrived;

        public float Speed
        {
            get => speed;
            set => speed = Mathf.Max(0f, value);
        }

        public void MoveTo(Vector3 target) => Target = target;

        public void Stop() => Target = null;

        private void Update()
        {
            if (!Target.HasValue)
                return;

            transform.position = Vector3.MoveTowards(transform.position, Target.Value, speed * Time.deltaTime);

            if (transform.position == Target.Value)
            {
                Target = null;
                Arrived?.Invoke();
            }
        }
    }
}
