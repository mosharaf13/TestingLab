using System;
using UnityEngine;
using UnityEngine.UI;

namespace TestingLab.Gameplay.UI
{
    /// <summary>Passive View for the end-of-level panel (win or lose).</summary>
    public sealed class ResultsView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text titleText;
        [SerializeField] private Text summaryText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Text continueLabel;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;

        public event Action ContinueClicked;
        public event Action RetryClicked;
        public event Action MenuClicked;

        public bool IsVisible => panel.activeSelf;
        public string Title => titleText.text;

        private void Awake()
        {
            continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            retryButton.onClick.AddListener(() => RetryClicked?.Invoke());
            menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
        }

        public void ShowWin(string title, string summary, string continueText)
        {
            Show(title, summary);
            continueLabel.text = continueText;
            continueButton.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(false);
        }

        public void ShowLoss(string title, string summary)
        {
            Show(title, summary);
            continueButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(true);
        }

        public void Hide() => panel.SetActive(false);

        private void Show(string title, string summary)
        {
            titleText.text = title;
            summaryText.text = summary;
            panel.SetActive(true);
        }
    }
}
