using System.Collections;
using NUnit.Framework;
using TestingLab.Gameplay.Player;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Gameplay/Player/PlayerController.cs
    //
    // Topic: replacing INPUT with a stub. We can't press W in a test, but the
    // controller reads an IPlayerInput, so we give it a ScriptedPlayerInput and
    // "hold right" from code.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class PlayerControllerTests
    {
        private GameObject _go;
        private PlayerController _player;
        private ScriptedPlayerInput _input;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("Player");
            _player = _go.AddComponent<PlayerController>(); // RequireComponent adds a Rigidbody
            _go.GetComponent<Rigidbody>().isKinematic = true;

            _input = new ScriptedPlayerInput();
            _player.InputSource = _input; // replaces the keyboard input set in Awake
            _player.ArenaHalfExtents = new Vector2(50f, 50f);
        }

        [TearDown]
        public void TearDown() => Object.Destroy(_go);

        [UnityTest]
        public IEnumerator HoldingRight_MovesPlayerRight()
        {
            _player.MoveSpeed = 10f;
            _input.Move = Vector2.right;

            yield return WaitFor.Frames(10);

            Assert.That(_go.transform.position.x, Is.GreaterThan(0f));
            Assert.That(_go.transform.position.z, Is.EqualTo(0f).Within(0.0001f));
        }

        [UnityTest]
        public IEnumerator NoInput_PlayerStaysStill()
        {
            _player.MoveSpeed = 10f;
            _input.Move = Vector2.zero;

            yield return WaitFor.Frames(10);

            Assert.That(_go.transform.position, Is.EqualTo(Vector3.zero));
        }

        [UnityTest]
        public IEnumerator Player_CannotLeaveTheArena()
        {
            _player.ArenaHalfExtents = new Vector2(1f, 1f);
            _player.MoveSpeed = 100f;
            _input.Move = Vector2.right;

            // Wait for TIME, not a number of frames: frame length varies (an unfocused
            // Editor can run 1 ms frames), so "10 frames" might not be far enough.
            // 0.25 s at speed 100 is 25 units: way past the wall at x = 1.
            yield return new WaitForSeconds(0.25f);

            Assert.That(_go.transform.position.x, Is.EqualTo(1f).Within(0.0001f));
        }
    }
}
