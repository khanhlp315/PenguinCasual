namespace Penguin
{
    public interface IScoreCaculator
    {
        event System.Action<long> OnScoreUpdate;
        event System.Action<long> OnScoreIncrease;

        long Score { get; }

        void OnPassingLayer(PedestalLayer pedestalLayer);
        void Update(float timeDelta);
    }
}