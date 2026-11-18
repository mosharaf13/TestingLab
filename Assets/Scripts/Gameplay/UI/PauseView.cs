using System;
using UnityEngine;
using UnityEngine.UI;

namespace TestingLab.Gameplay.UI
{
    /// <summary>Passive View for the pause overlay. Shows/hides a child panel.</summary>
    public sealed class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;

        public event Action ResumeClicked;
        public event Action QuitClicked;

        public bool IsVisible => panel.activeSelf;

        private void Awake()
        {
            resumeButton.onClick.AddListener(() => ResumeClicked?.Invoke());
            quitButton.onClick.AddListener(() => QuitClicked?.Invoke());
        }

        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}
