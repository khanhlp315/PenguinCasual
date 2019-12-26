using System;

namespace Penguin
{
    public class SimpleScoreCalculator : IScoreCaculator
    {
        public event Action<long> OnScoreUpdate;
        public event Action OnComboActive;
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
        /// Check if combo is active
        /// </summary>
        public bool HasActiveCombo
        {
            get
            {
                return _scoreSetting != null ?
                                    _continousFloorCount >= _scoreSetting.numFloorActiveCombo : false;
            }
        }

        private int _continousFloorCount = 0;

        private PedestalType _previousPedestalType;

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
            _previousPedestalType = PedestalType.None;
            foreach (var item in pedestalLayer.pedestalLayers)
            {
                if (item.type != PedestalType.DeadZone_01 &&
                    item.type != PedestalType.Wall_01)
                {
                    _previousPedestalType = item.type;
                    break;
                }
            }

            _continousFloorCount += 1;
            if (_continousFloorCount >= _scoreSetting.numFloorActiveCombo)
            {
                OnComboActive?.Invoke();
            }
        }

        public void OnLandingLayer(Pedestal pedestal)
        {
            if (_continousFloorCount >= _scoreSetting.numFloorActiveCombo)
            {
                long increaseScore = _scoreSetting.basicScore * _scoreSetting.comboMultiply * _continousFloorCount;

                Score += increaseScore;
                OnScoreIncrease?.Invoke(increaseScore);
                OnScoreUpdate?.Invoke(Score);
            }
            else if (_continousFloorCount == 1)
            {
                long increaseScore = 0;

                switch (_previousPedestalType)
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

            _continousFloorCount = 0;
        }

        public void Update(float timeDelta)
        {
        }
    }
}