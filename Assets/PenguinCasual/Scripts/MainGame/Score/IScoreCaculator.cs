namespace Penguin
{
    public interface IScoreCaculator
    {
        event System.Action<long> OnScoreUpdate;
        event System.Action<long> OnScoreIncrease;
        event System.Action OnComboActive;

        long Score { get; }
        bool HasActiveCombo { get; }

        void OnPassingLayer(PedestalLayer pedestalLayer);
        void OnLandingLayer(Pedestal pedestalLayer);
        void Update(float timeDelta);
    }
}