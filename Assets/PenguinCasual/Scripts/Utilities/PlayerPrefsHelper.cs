using System;
using System.Globalization;
using UnityEngine;

namespace PenguinCasual.Scripts.Utilities
{
    public class PlayerPrefsKeys
    {
        public const string HIGH_SCORE = "HIGH_SCORE";
        public const string WATCH_ADS_TIMES_TODAY = "WATCH_ADS_TIMES_TODAY";
        public const string CURRENT_DATE = "CURRENT_DATE";

        public const string GAMES_PLAYED = "GAMES_PLAYED";
        public const string WATCH_ADS_TIMES = "WATCH_ADS_TIMES";
        public const string DAYS_PLAYED = "WATCH_ADS_TIMES";
        public const string TOTAL_SCORE = "TOTAL_SCORE";
    }
    
    public class PlayerPrefsHelper
    {
        public static int WatchAdsTimesPerDay = 5;
        
        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(PlayerPrefsKeys.HIGH_SCORE);
        }
        
        public static void UpdateHighScore(int highScore)
        { 
            PlayerPrefs.SetInt(PlayerPrefsKeys.HIGH_SCORE, highScore);
        }

        public static bool CanWatchAds()
        {
            return WatchAdsTimesToday() < WatchAdsTimesPerDay;
        }

        public static int WatchAdsTimesToday()
        {
            var today = DateTime.Today.ToString(CultureInfo.InvariantCulture);
            var currentDay = PlayerPrefs.GetString(PlayerPrefsKeys.CURRENT_DATE);
            int watchAdsTimesToday = 0;
            if (today != currentDay)
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.CURRENT_DATE, today);
                PlayerPrefs.SetInt(PlayerPrefsKeys.WATCH_ADS_TIMES_TODAY, 0);
                var daysPlayed = PlayerPrefs.GetInt(PlayerPrefsKeys.DAYS_PLAYED, 0);
                PlayerPrefs.SetInt(PlayerPrefsKeys.DAYS_PLAYED, ++daysPlayed);
            }
            else
            {
                watchAdsTimesToday = PlayerPrefs.GetInt(PlayerPrefsKeys.WATCH_ADS_TIMES_TODAY);
            }

            return watchAdsTimesToday;
        }

        public static void CountGamesPlayed()
        {
            var gamesPlayed = PlayerPrefs.GetInt(PlayerPrefsKeys.GAMES_PLAYED, 0);
            gamesPlayed++;
            PlayerPrefs.SetInt(PlayerPrefsKeys.GAMES_PLAYED, gamesPlayed);
        }
        
        public static int GetGamesPlayed()
        {
            var gamesPlayed = PlayerPrefs.GetInt(PlayerPrefsKeys.GAMES_PLAYED, 0);
            return gamesPlayed;
        }
        
        public static void CountWatchAdsTimes()
        {
            var watchAdsTimes = PlayerPrefs.GetInt(PlayerPrefsKeys.WATCH_ADS_TIMES, 0);
            var watchAdsTimesToday = PlayerPrefs.GetInt(PlayerPrefsKeys.WATCH_ADS_TIMES_TODAY, 0);
            
            Debug.Log(watchAdsTimesToday);
            
            PlayerPrefs.SetInt(PlayerPrefsKeys.WATCH_ADS_TIMES, ++watchAdsTimes);
            PlayerPrefs.SetInt(PlayerPrefsKeys.WATCH_ADS_TIMES_TODAY, ++watchAdsTimesToday);
        }
        
        public static int GetWatchAdsTimes()
        {
            var watchAdsTimes = PlayerPrefs.GetInt(PlayerPrefsKeys.WATCH_ADS_TIMES, 0);
            return watchAdsTimes;
        }

        public static void CheckDate()
        {
            var today = DateTime.Today.ToString(CultureInfo.InvariantCulture);
            var currentDay = PlayerPrefs.GetString(PlayerPrefsKeys.CURRENT_DATE);
            if (today != currentDay)
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.CURRENT_DATE, today);
                PlayerPrefs.SetInt(PlayerPrefsKeys.WATCH_ADS_TIMES_TODAY, 0);
                var daysPlayed = PlayerPrefs.GetInt(PlayerPrefsKeys.DAYS_PLAYED, 0);
                PlayerPrefs.SetInt(PlayerPrefsKeys.DAYS_PLAYED, ++daysPlayed);
            }
        }

        public static void AddToTotalScore(int score)
        {
            var totalScore = PlayerPrefs.GetInt(PlayerPrefsKeys.TOTAL_SCORE, 0);
            totalScore += score;
            PlayerPrefs.SetInt(PlayerPrefsKeys.TOTAL_SCORE, totalScore);
            PlayerPrefs.Save();
        }
    }
}