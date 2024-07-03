namespace NetBootcamp.Core.CrossCuttingConcerns.Caching
{
    public abstract class CacheService
    {
        public abstract void AddCache<T>(string key, T value, TimeSpan? expireTime = null);
        public abstract void RemoveCache(string key);
        public abstract T GetCache<T>(string key);
        public abstract bool IsExistCache<T>(string key, out T data);
        public string CreateCacheKey(string serviceName, string methodName, string entityName)
        {
            return $"{serviceName} --> {methodName} --> {entityName}";
        }
    }
}
