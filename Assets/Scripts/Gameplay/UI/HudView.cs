using System;
using TestingLab.Core.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace TestingLab.Gameplay.UI
{
    /// <summary>
    /// Passive View for the in-game HUD. All text comes from HudFormatter (Core),
    /// which is unit tested. Values are cached so strings are only rebuilt when
    /// something changed (no garbage every frame).
    /// </summary>
    public sealed class HudView : MonoBehaviour
    {
        private const float DamagePopupSeconds = 0.8f;

        [SerializeField] private Text levelText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text coinsText;
        [SerializeField] private Text timerText;
        [SerializeField] private Text healthText;
        [SerializeField] private RectTransform healthFill;
        [SerializeField] private Text damageText;
        [SerializeField] private Button pauseButton;

        private int _score = -1, _multiplier = -1, _coins = -1, _goal = -1, _seconds = -1, _health = -1, _maxHealth = -1;
        private float _damageHideAt;

        public event Action PauseClicked;

        public string ScoreText => scoreText.text;
        public string CoinsText => coinsText.text;
        public string HealthText => healthText.text;

        private void Awake()
        {
            pauseButton.onClick.AddListener(() => PauseClicked?.Invoke());
            damageText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (damageText.gameObject.activeSelf && Time.unscaledTime >= _damageHideAt)
                damageText.gameObject.SetActive(false);
        }

        public void SetLevel(string text) => levelText.text = text;

        public void SetScore(int score, int multiplier)
        {
            if (score == _score && multiplier == _multiplier)
                return;
            _score = score;
            _multiplier = multiplier;
            scoreText.text = HudFormatter.Score(score, multiplier);
        }

        public void SetCoins(int collected, int goal)
        {
            if (collected == _coins && goal == _goal)
                return;
            _coins = collected;
            _goal = goal;
            coinsText.text = HudFormatter.Coins(collected, goal);
        }

        public void SetTimeRemaining(float seconds)
        {
            int whole = Mathf.CeilToInt(seconds);
            if (whole == _seconds)
                return;
            _seconds = whole;
            timerText.text = TimeFormatter.MinutesSeconds(seconds);
            timerText.color = seconds <= 10f ? UiTheme.DangerButton : UiTheme.Text;
        }

        public void SetHealth(int current, int max)
        {
            if (current == _health && max == _maxHealth)
                return;
            _health = current;
            _maxHealth = max;
            healthText.text = HudFormatter.Health(current, max);
            healthFill.anchorMax = new Vector2(max > 0 ? Mathf.Clamp01((float)current / max) : 0f, 1f);
        }

        public void ShowDamage(int amount, bool critical)
        {
            damageText.text = HudFormatter.Damage(amount, critical);
            damageText.color = critical ? UiTheme.Accent : UiTheme.DangerButton;
            damageText.gameObject.SetActive(true);
            _damageHideAt = Time.unscaledTime + DamagePopupSeconds;
        }
    }
}
