using System;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class UnlockData
    {
        const string PLAY = "プレイ回数";
        const string WATCH_ADS = "動画再⽣数";
        const string DAYS_PLAYED = "⽇数解放";
        const string TOTAL_SCORE = "スコア獲得数";
        
        [SerializeField]
        private int id;

        [SerializeField] 
        private string unlock_action_name;

        [SerializeField] 
        private int condition;

        public int Id => id;
        public string UnlockActionName => unlock_action_name;
        public int Condition => condition;

        public bool IsUnlocked()
        {
            switch (UnlockActionName)
            {
                case PLAY:
                    var playTimes = PlayerPrefsHelper.GetGamesPlayed();
                    return playTimes >= Condition;
                case WATCH_ADS:
                    var watchAdsTimes = PlayerPrefsHelper.GetWatchAdsTimes();
                    return watchAdsTimes >= Condition;
                case DAYS_PLAYED:
                    var daysPlayed = PlayerPrefsHelper.GetDaysPlayed();
                    return daysPlayed >= Condition;
                case TOTAL_SCORE:
                    var totalScore = PlayerPrefsHelper.GetTotalScore();
                    return totalScore >= Condition;
            }

            return false;
        }
    }
    
    [Serializable]
    public class UnlockDataResponse : ResponseBodyWithMultipleEntities<UnlockData>
    {
        public static UnlockDataResponse FromJson(string jsonString)
        {
            return JsonUtility.FromJson<UnlockDataResponse>(jsonString);
        }
    }
}