using System;

namespace Penguin
{
    public class SimpleScoreCalculator : IScoreCaculator
    {
        public event Action<long> OnScoreUpdate;
        public event Action<long> OnScoreIncrease;

        /// <summary>
        /// Preference instance for score setting
        /// </summary>
        private ScoreSetting _scoreSetting;

        /// <summary>
        /// Current score
        /// </summary>
        public long Score { get; protected set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scoreSetting"></param>
        public SimpleScoreCalculator(ScoreSetting scoreSetting)
        {
            _scoreSetting = scoreSetting;
            Score = 0;
        }

        public void OnPassingLayer(PedestalLayer pedestalLayer)
        {
            PedestalType pedestalType = PedestalType.None;
            foreach (var item in pedestalLayer.pedestalLayers)
            {
                if (item.type != PedestalType.DeadZone_01 &&
                    item.type != PedestalType.Wall_01)
                {
                    pedestalType = item.type;
                    break;
                }
            }

            long increaseScore = 0;

            switch (pedestalType)
            {
                case PedestalType.Pedestal_01:
                    increaseScore = _scoreSetting.basicScore * _scoreSetting.floorTypeNoFishMultiply + _scoreSetting.floorTypeNoFishIncrease;
                    break;
                case PedestalType.Pedestal_01_1_Fish:
                    increaseScore = _scoreSetting.basicScore * _scoreSetting.floorTypeOneFishMultiply + _scoreSetting.floorTypeOneFishIncrease;
                    break;
                case PedestalType.Pedestal_01_3_Fish:
                    increaseScore = _scoreSetting.basicScore * _scoreSetting.floorTypeThreeFishMultiply + _scoreSetting.floorTypeThreeFishIncrease;
                    break;
            }

            Score += increaseScore;
            OnScoreIncrease?.Invoke(increaseScore);
            OnScoreUpdate?.Invoke(Score);
        }

        public void Update(float timeDelta)
        {
        }
    }
}