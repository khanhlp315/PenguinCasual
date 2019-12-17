using UnityEngine;

namespace Analytics
{
    public class DummyAnalyticsSystem : IAnalyticSystem
    {
        #region IAnalyticSystem implementation

        public void LogScreen (string screen)
        {
            Debug.Log ("<color=green>[Analytic] - Log Screen - " + screen + "</color>");
        }

        public void LogEvent (string eventName, params AnalyticsParameter[] paremeters)
        {
            string paramsString = "(";
            if (paremeters != null) {
                foreach (var item in paremeters) {
                    paramsString += string.Format ("(\"{0}\" : {1}) ", item.Name, item.Value);
                }
                paramsString += ")";
            }
			
            Debug.Log ("\"<color=green>[Analytic] - Log Event - " + eventName + " - " + paramsString + "</color>");
        }

        public void LogException (string exceptionDescription, bool isFatal)
        {
            Debug.LogError ("[Analytic] - Log Exception - " + exceptionDescription);
        }

        #endregion
    }

}