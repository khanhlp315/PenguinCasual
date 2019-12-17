using Firebase.Analytics;

namespace Analytics
{
    public class FirebaseAnalyticsSystem : IAnalyticSystem 
    {
        #region IAnalyticSystem implementation
        public void LogScreen (string screen)
        {
            FirebaseAnalytics.SetCurrentScreen (screen, screen);
        }

        public void LogEvent (string eventName, params AnalyticsParameter[] paremeters)
        {
            if (paremeters == null) {
                FirebaseAnalytics.LogEvent (eventName);
            } else {
                var eventParams = new Parameter[paremeters.Length];
                int index = 0;
                foreach (var item in paremeters) {
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
        #endregion
		
    }	
}