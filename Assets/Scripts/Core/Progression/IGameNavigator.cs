namespace TestingLab.Core.Progression
{
    // Pattern: Dependency Inversion. Core decides WHERE to go next; the Gameplay
    // layer (SceneNavigator) knows HOW to load a Unity scene. Tests pass a
    // SpyGameNavigator that just records which method was called.

    public interface IGameNavigator
    {
        void GoToMainMenu();
        void GoToArena();
        void GoToShop();
    }
}
