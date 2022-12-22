namespace Shoelace.Bejeweld.Components
{
    public interface ILevelHudView
    {
        public void UpdateProgress(int score);
        void SetLevel(Level level);
    }
}