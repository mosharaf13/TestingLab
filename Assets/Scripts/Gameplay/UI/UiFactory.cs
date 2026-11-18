using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TestingLab.Gameplay.UI
{
    // ─────────────────────────────────────────────────────────────────────────
    // Pattern: Factory. Creates consistently styled uGUI elements from code.
    // Used by the editor scene builder (to lay out the scenes) and at runtime by
    // ShopView (to create one row per shop item).
    // ─────────────────────────────────────────────────────────────────────────

    public static class UiFactory
    {
        private static Font _font;

        public static Font DefaultFont => _font != null ? _font : _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        public static Canvas CreateCanvas(string name, int sortingOrder = 0)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            return canvas;
        }

        public static EventSystem EnsureEventSystem()
        {
            var existing = Object.FindFirstObjectByType<EventSystem>();
            if (existing != null)
                return existing;

            var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            return go.GetComponent<EventSystem>();
        }

        public static RectTransform CreateRect(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return (RectTransform)go.transform;
        }

        public static Image CreatePanel(Transform parent, string name, Color color)
        {
            var rect = CreateRect(parent, name);
            var image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            return image;
        }

        public static Text CreateText(Transform parent, string name, string text, int size,
            TextAnchor alignment = TextAnchor.MiddleCenter, Color? color = null)
        {
            var rect = CreateRect(parent, name);
            var label = rect.gameObject.AddComponent<Text>();
            label.font = DefaultFont;
            label.text = text;
            label.fontSize = size;
            label.alignment = alignment;
            label.color = color ?? UiTheme.Text;
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            label.raycastTarget = false;
            return label;
        }

        public static Button CreateButton(Transform parent, string name, string label, Color? color = null, int fontSize = UiTheme.BodySize)
        {
            var image = CreatePanel(parent, name, Color.white);
            var button = image.gameObject.AddComponent<Button>();
            var baseColor = color ?? UiTheme.Button;
            var colors = button.colors;
            colors.normalColor = baseColor;
            colors.highlightedColor = Color.Lerp(baseColor, Color.white, 0.2f);
            colors.pressedColor = Color.Lerp(baseColor, Color.black, 0.25f);
            colors.selectedColor = baseColor;
            button.colors = colors;

            var text = CreateText(image.transform, "Label", label, fontSize);
            Stretch(text.rectTransform);

            var layout = image.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = 64f;
            layout.preferredHeight = 64f;
            return button;
        }

        public static InputField CreateInputField(Transform parent, string name, string placeholder)
        {
            var image = CreatePanel(parent, name, new Color(1f, 1f, 1f, 0.95f));
            var input = image.gameObject.AddComponent<InputField>();

            var text = CreateText(image.transform, "Text", string.Empty, UiTheme.BodySize, TextAnchor.MiddleLeft, Color.black);
            text.supportRichText = false;
            Stretch(text.rectTransform, 12f);

            var hint = CreateText(image.transform, "Placeholder", placeholder, UiTheme.BodySize, TextAnchor.MiddleLeft, new Color(0f, 0f, 0f, 0.45f));
            hint.fontStyle = FontStyle.Italic;
            Stretch(hint.rectTransform, 12f);

            input.textComponent = text;
            input.placeholder = hint;
            var layout = image.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = 56f;
            layout.preferredHeight = 56f;
            return input;
        }

        public static VerticalLayoutGroup AddVerticalLayout(GameObject go, float spacing = 16f, int padding = 24)
        {
            var layout = go.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = new RectOffset(padding, padding, padding, padding);
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return layout;
        }

        public static HorizontalLayoutGroup AddHorizontalLayout(GameObject go, float spacing = 12f)
        {
            var layout = go.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            // true here would also make the ROW itself flexible in its parent layout,
            // so rows would stretch to fill the panel.
            layout.childForceExpandHeight = false;
            return layout;
        }

        public static void SetPreferredSize(Component component, float width = -1f, float height = -1f, float flexibleWidth = -1f)
        {
            // Don't use ?? with Unity objects: a missing component can be a "fake null"
            // that ?? treats as a real object. TryGetComponent avoids the trap.
            if (!component.TryGetComponent(out LayoutElement layout))
                layout = component.gameObject.AddComponent<LayoutElement>();
            if (width >= 0f) layout.preferredWidth = width;
            if (height >= 0f)
            {
                layout.preferredHeight = height;
                layout.minHeight = height;
                layout.flexibleHeight = 0f; // a fixed height must not soak up spare space
            }
            if (flexibleWidth >= 0f) layout.flexibleWidth = flexibleWidth;
        }

        /// <summary>Fill the parent, with an optional inset on every side.</summary>
        public static void Stretch(RectTransform rect, float inset = 0f)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(inset, inset);
            rect.offsetMax = new Vector2(-inset, -inset);
        }

        /// <summary>Anchor to a point of the parent (0..1 coordinates) with a fixed size.</summary>
        public static void Place(RectTransform rect, Vector2 anchor, Vector2 size, Vector2 offset = default)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.sizeDelta = size;
            rect.anchoredPosition = offset;
        }
    }
}
