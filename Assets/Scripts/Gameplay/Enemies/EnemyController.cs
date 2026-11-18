using UnityEngine;

namespace TestingLab.Gameplay.Enemies
{
    /// <summary>
    /// Humble Object: each frame asks ChaseBrain where to go and tells Mover to go
    /// there. All the decisions are tested in ChaseBrainTests; this class just
    /// connects them to the scene.
    /// </summary>
    [RequireComponent(typeof(Mover))]
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float detectRange = 7f;
        [SerializeField, Min(0.1f)] private float loseRange = 10f;

        private Mover _mover;
        private ChaseBrain _brain;
        private Vector3 _home;

        public Transform Target { get; private set; }
        public ChaseState State => _brain?.State ?? ChaseState.Idle;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _brain = new ChaseBrain(detectRange, Mathf.Max(detectRange, loseRange));
            _home = transform.position;
        }

        public void Configure(Transform target, float speed)
        {
            Target = target;
            _mover.Speed = speed;
        }

        private void Update()
        {
            if (Target == null)
            {
                _mover.Stop();
                return;
            }

            _mover.MoveTo(_brain.ChooseDestination(transform.position, Target.position, _home));
        }
    }
}
