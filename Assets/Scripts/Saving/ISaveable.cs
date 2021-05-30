namespace CultGame.Saving
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
