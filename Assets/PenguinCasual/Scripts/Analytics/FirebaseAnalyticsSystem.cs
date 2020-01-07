using Firebase.Analytics;

namespace Penguin.Analytics
{
    public class FirebaseAnalyticsSystem : IAnalyticSystem 
    {
        #region IAnalyticSystem implementation
        public void LogScreen (string screen)
        {
            FirebaseAnalytics.SetCurrentScreen (screen, screen);
        }

        public void LogEvent (string eventName, params AnalyticsParameter[] parameters)
        {
            if (parameters == null) {
                FirebaseAnalytics.LogEvent (eventName);
            } else {
                var eventParams = new Parameter[parameters.Length];
                int index = 0;
                foreach (var item in parameters) {
                    Parameter p = new Parameter (item.Name, item.Value);
                    eventParams [index] = p;
                    index += 1;
                }

                FirebaseAnalytics.LogEvent (eventName, eventParams);
            }
        }
        public void LogException (string exceptionDescription, bool isFatal)
        {
        }

        public void SetProperties(params AnalyticsParameter[] parameters)
        {
            foreach (var item in parameters)
            {
                FirebaseAnalytics.SetUserProperty(item.Name, item.Value);
            }
        }

        #endregion
		
    }	
}