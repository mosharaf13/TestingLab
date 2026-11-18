namespace TestingLab.Core.Session
{
    public enum GamePhase
    {
        Ready,
        Playing,
        Paused,
        Won,
        Lost
    }

    public enum LoseReason
    {
        None,
        TimeUp,
        PlayerDied
    }
}
