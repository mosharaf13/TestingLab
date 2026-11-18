using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TestingLab.Tests.Shared.Support
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED (UI tests)
    //
    // Clicks a uGUI Button by its GameObject name, the way the EventSystem would.
    // It refuses to click hidden or disabled buttons, because a player can't
    // either. That turns "the button exists but is invisible" into a test failure.
    // ─────────────────────────────────────────────────────────────────────────

    public static class UiTestHelpers
    {
        public static Button FindButton(string name)
        {
            var button = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(b => b.name == name);
            if (button == null)
                throw new InvalidOperationException($"No button named '{name}' in the loaded scenes.");
            return button;
        }

        public static void Click(string buttonName)
        {
            var button = FindButton(buttonName);
            if (!button.gameObject.activeInHierarchy)
                throw new InvalidOperationException($"Button '{buttonName}' is hidden, a player couldn't click it.");
            if (!button.IsInteractable())
                throw new InvalidOperationException($"Button '{buttonName}' is not interactable.");

            var eventData = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left };
            ExecuteEvents.Execute(button.gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }

        public static bool IsVisible(string buttonName) => FindButton(buttonName).gameObject.activeInHierarchy;
    }
}
