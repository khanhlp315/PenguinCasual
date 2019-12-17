namespace Penguin
{
    public class CacheData
    {
        public readonly object data;
        public readonly long expireTime;
        public readonly string category;

        public CacheData(object data, long expireTime, string category)
        {
            this.data = data;
            this.expireTime = expireTime;
            this.category = category;
        }
    }
}