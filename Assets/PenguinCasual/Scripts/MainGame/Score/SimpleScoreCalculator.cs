using System;
using System.Diagnostics;

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

        //private int _continousFloorCount = 0;
        private int _continousPowerupFloorCount = 0;

        private bool _hasPassLayer;
        private bool _preventUpdateScore;

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

        public void OnPassingLayer(bool hasPowerup, PedestalLayer pedestalLayer)
        {
            if (hasPowerup)
            {
                UnityEngine.Debug.LogError("Passing layer Powerup : " + _continousPowerupFloorCount);
                if (_continousPowerupFloorCount < _scoreSetting.passingFloorMultiplies.Count)
                {
                    var increaseScore = _scoreSetting.basicScore * _scoreSetting.passingFloorMultiplies[_continousPowerupFloorCount];
                    UnityEngine.Debug.LogError("Powerup Increase score : " + increaseScore);
                    UnityEngine.Debug.LogError("Powerup multiply : " + _scoreSetting.passingFloorMultiplies[_continousPowerupFloorCount]);
                    Score += increaseScore;
                    OnScoreIncrease?.Invoke(increaseScore);
                    OnScoreUpdate?.Invoke(Score);
                }

                _continousPowerupFloorCount += 1;
            }
            else
            {
                _previousPedestalType = PedestalType.None;
                foreach (var item in pedestalLayer.pedestalInfos)
                {
                    if (item.type == PedestalType.Pedestal_01 ||
                        item.type == PedestalType.Pedestal_01_1_Fish ||
                        item.type == PedestalType.Pedestal_01_3_Fish)
                    {
                        _previousPedestalType = item.type;
                        break;
                    }
                }
            }

            _hasPassLayer = true;
        }

        public void OnLandingLayer(bool hasCombo, int comboCount, Pedestal pedestal)
        {
            if (_preventUpdateScore)
            {
                _preventUpdateScore = false;
                _hasPassLayer = false;
                _continousPowerupFloorCount = 0;
                return;
            }

            if (!_hasPassLayer)
                return;

            if (hasCombo)
            {
                long increaseScore = _scoreSetting.basicScore * _scoreSetting.comboMultiply * comboCount;

                UnityEngine.Debug.LogError("Passing layer combo : " + increaseScore);
                UnityEngine.Debug.LogError("Passing layer count : " + comboCount);

                Score += increaseScore;
                OnScoreIncrease?.Invoke(increaseScore);
                OnScoreUpdate?.Invoke(Score);
            }
            else
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

                UnityEngine.Debug.LogError("Normal score : " + increaseScore);

                Score += increaseScore;
                OnScoreIncrease?.Invoke(increaseScore);
                OnScoreUpdate?.Invoke(Score);
            }

            _hasPassLayer = false;
            _continousPowerupFloorCount = 0;
        }

        public void Update(float timeDelta)
        {
        }

        public void PreventUpdateScoreOnNextLanding()
        {
            _preventUpdateScore = true;
        }
    }
}