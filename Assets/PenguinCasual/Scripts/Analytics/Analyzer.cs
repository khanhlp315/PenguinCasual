namespace Analytics
{
    public class Analyzer
    {
        static AnalyticsSystem _analytics = null;
        public static AnalyticsSystem Analytics{
            get{
                if (_analytics == null) {
                    _analytics = new AnalyticsSystem ();
                    _analytics.Add<DummyAnalyticsSystem> ();
					_analytics.Add<FirebaseAnalyticsSystem> ();
                }
                return _analytics;
            }
        }

        public static void SetAnalyticSystem ( AnalyticsSystem sys )
        {
            _analytics = sys;
        }
    }
}