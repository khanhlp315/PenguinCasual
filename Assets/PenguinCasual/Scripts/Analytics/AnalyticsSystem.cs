using System.Collections.Generic;
using UnityEngine;

namespace Analytics
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
            public static void Replay()
            {
                Analyzer.Analytics.LogEvent ("replay");
            }
        }
    }
    
    
}