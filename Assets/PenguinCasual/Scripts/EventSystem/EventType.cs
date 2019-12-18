namespace Penguin
{
    internal static class EventType<T> where T : struct, IEvent
    {
        private static int _index;
        public static int Index => _index;

        static EventType()
        {
            _index = -1;
        }

        public static int SyncIndex()
        {
            _index = TypeManager<IEvent>.IndexOf(typeof(T));
            return _index;
        }
    }
}