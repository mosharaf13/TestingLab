using UnityEngine;

namespace TestingLab.Gameplay.UI
{
    /// <summary>
    /// Press F1 in any scene to see which tests cover what you're looking at.
    /// A teaching aid: connects "the thing on screen" to "the test that protects it".
    /// Uses IMGUI (OnGUI) because it's a debug overlay, not part of the game's UI.
    /// </summary>
    public sealed class LabNotesOverlay : MonoBehaviour
    {
        [SerializeField] private string title = "Lab notes";
        [SerializeField, TextArea(6, 30)] private string notes;

        private bool _visible;
        private GUIStyle _boxStyle;

        public string Notes
        {
            get => notes;
            set => notes = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }

        private void OnGUI()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.F1)
            {
                _visible = !_visible;
                e.Use();
            }

            _boxStyle ??= new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.UpperLeft,
                wordWrap = true,
                fontSize = 14,
                padding = new RectOffset(12, 12, 12, 12),
                richText = true
            };

            if (!_visible)
            {
                GUI.Label(new Rect(Screen.width - 170, Screen.height - 28, 160, 24), "F1: Lab notes");
                return;
            }

            float width = Mathf.Min(560f, Screen.width - 40f);
            GUI.Box(new Rect(20f, 20f, width, Screen.height - 40f), $"<b>{title}</b>  (F1 to close)\n\n{notes}", _boxStyle);
        }
    }
}
