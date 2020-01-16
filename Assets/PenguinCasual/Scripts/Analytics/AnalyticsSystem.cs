using System.Collections.Generic;
using UnityEngine;

namespace Penguin.Analytics
{
    public class AnalyticsParameter
    {
        public AnalyticsParameter(string name , long value)
        {
            this.Name = name;
            this.Value = value.ToString();
        }

        public AnalyticsParameter(string name , string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public AnalyticsParameter(string name , double value)
        {
            this.Name = name;
            this.Value = value.ToString();
        }

        public string Name;
        public string Value;
    }
	
    public interface IAnalyticSystem
    {
        void LogScreen( string screen );
        void LogEvent (string eventName, params AnalyticsParameter[] parameters);
        void LogException (string exceptionDescription, bool isFatal);
        void SetProperties(params AnalyticsParameter[] parameters);
    }

    /// <summary>
    /// Analytic system is design with compose a lot of system and use it at same time
    /// </summary>
    public sealed class AnalyticsSystem: IAnalyticSystem
    {
        private Dictionary<string, IAnalyticSystem> _managedSystem = new Dictionary<string, IAnalyticSystem>();

        public void LogEvent(string eventName, params AnalyticsParameter[] paremeters)
        {
            foreach (var item in _managedSystem) {
                item.Value.LogEvent (eventName, paremeters);
            }
        }

        public void LogScreen( string screen )
        {
            foreach (var item in _managedSystem) {
                item.Value.LogScreen (screen);
            }
        }

        public void LogException(string exceptionDescription, bool isFatal)
        {
            foreach (var item in _managedSystem) {
                item.Value.LogException (exceptionDescription, isFatal);
            }
        }

        public void SetProperties(params AnalyticsParameter[] parameters)
        {
            foreach (var item in _managedSystem) {
                item.Value.SetProperties (parameters);
            }
        }

        public AnalyticsSystem Add<TValue>()  where TValue : IAnalyticSystem , new()
        {    
            var system = new TValue ();
            string key = system.GetType().ToString();
            if (_managedSystem.ContainsKey (key)){
                Debug.LogErrorFormat ("System {0} already exist", key);
                return this;
            }
            _managedSystem.Add (key, system);
            return this;
        }
        
        public void Remove<TValue>()
        {
            string key = typeof(TValue).ToString ();
            _managedSystem.Remove (key);
        }
        
        public IAnalyticSystem Get<TValue>()
            where TValue : IAnalyticSystem
        {
            string key = typeof(TValue).ToString();
            if (_managedSystem.ContainsKey (key)) {
                return _managedSystem [key];
            }

            Debug.LogErrorFormat ("System {0} did not exits", key);
            return default(IAnalyticSystem);
        }
    }

    public static class StandardProperties
    {
        public static void SetDaysPlayed(int daysPlayed)
        {
            Analyzer.Analytics.SetProperties(new AnalyticsParameter("days_played", daysPlayed));
        }
        
        public static void SetBestScore(int bestScore)
        {
            Analyzer.Analytics.SetProperties(new AnalyticsParameter("best_score", bestScore));
        }
        
        public static void SetWatchAdsTimes(int watchAdsTimes)
        {
            Analyzer.Analytics.SetProperties(new AnalyticsParameter("watch_ads_times", watchAdsTimes));
        }
    }

    public static class StandardEvent
    {
        public static class App
        {
            public static void AppLaunch( )
            {
                Analyzer.Analytics.LogEvent ("app_launch");
            }
        }

        public static class GameProgress
        {
            public static void StartGame(int backgroundId, int skinId)
            {
                Analyzer.Analytics.LogEvent ("start_game", new AnalyticsParameter("background", backgroundId),
                    new AnalyticsParameter("skin", skinId));
            }
            
            public static void Revive(int reviveTimes)
            {
                Analyzer.Analytics.LogEvent ("revive", new AnalyticsParameter("times", reviveTimes));
            }

            public static void BestScore(int score)
            {
                Analyzer.Analytics.LogEvent ("best_score", new AnalyticsParameter("score", score));
            }
        }
    }
    
    
}