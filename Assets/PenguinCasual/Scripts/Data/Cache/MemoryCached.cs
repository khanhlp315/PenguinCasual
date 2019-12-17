using System.Collections.Generic;
using Penguin.Utilities;

namespace Penguin
{
    public static class MemCached
    {
        // Use max int instead of max long, because of overflow long when plus
        // Max int is still big enough to represent for infinity
        const long INFINITY = int.MaxValue;
        static Dictionary<string, CacheData> _cacheData = new Dictionary<string, CacheData>();

        static public void Set(string key, object data, long lifespan = INFINITY)
        {
            Set(key, data, lifespan, null);
        }

        static public void Set(string key, object data, string category)
        {
            Set(key, data, INFINITY, category);
        }

        static public void Set(string key, object data, long lifespan, string category)
        {
            if (string.IsNullOrEmpty(key) || data == null)
                return;
            
            if (_cacheData.ContainsKey(key))
                _cacheData.Remove(key);

            long expireTime = lifespan + DateTimeUtil.TimeSinceEpoch();
            _cacheData.Add(key, new CacheData(data, expireTime, category));
        }

        static public T Get<T>(string key, bool isRemove = false)
        {
            CheckExpiredData();

            CacheData cacheData;
            _cacheData.TryGetValue(key, out cacheData);

            if (isRemove)
                _cacheData.Remove(key);

            if (cacheData != null)
                return (T)cacheData.data;
            else
                return default(T);
        }

        static public void Clear(string category)
        {
            List<string> removeKeys = new List<string>();

            foreach (var pair in _cacheData)
            {
                if (pair.Value.category.Equals(category))
                {
                    removeKeys.Add(pair.Key);
                }
            }

            foreach (var key in removeKeys)
            {
                _cacheData.Remove(key);
            }
        }

        static void CheckExpiredData()
        {
            long now = DateTimeUtil.TimeSinceEpoch();
            List<string> removeKeys = new List<string>();

            foreach (var pair in _cacheData)
            {
                if (now > pair.Value.expireTime)
                {
                    removeKeys.Add(pair.Key);
                }
            }

            foreach (var key in removeKeys)
            {
                _cacheData.Remove(key);
            }
        }
    }
}