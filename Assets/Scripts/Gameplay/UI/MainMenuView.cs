using System;
using UnityEngine;
using UnityEngine.UI;

namespace TestingLab.Gameplay.UI
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED (UI tests)
    // Tested by:      Tests/PlayMode/Level3_Advanced/MainMenuSceneTests.cs
    // Pattern:        Passive View (MVP). The view has NO game logic: it shows what
    //                 it's told and raises events when buttons are clicked.
    //                 MainMenuController decides what those clicks mean.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Text continueLabel;
        [SerializeField] private Button quitButton;
        [SerializeField] private Text bestScoreText;
        [SerializeField] private Text messageText;

        public event Action PlayClicked;
        public event Action ContinueClicked;
        public event Action QuitClicked;

        public string BestScoreText => bestScoreText.text;
        public bool IsContinueVisible => continueButton.gameObject.activeSelf;
        public string MessageText => messageText.text;

        private void Awake()
        {
            playButton.onClick.AddListener(() => PlayClicked?.Invoke());
            continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            quitButton.onClick.AddListener(() => QuitClicked?.Invoke());
        }

        public void SetBestScore(int bestScore) => bestScoreText.text = $"Best score: {bestScore}";

        public void SetContinue(bool visible, string label)
        {
            continueButton.gameObject.SetActive(visible);
            continueLabel.text = label;
        }

        public void SetMessage(string message)
        {
            messageText.text = message ?? string.Empty;
            messageText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }
    }
}
