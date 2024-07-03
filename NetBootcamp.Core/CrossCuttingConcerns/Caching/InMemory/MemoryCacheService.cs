using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetBootcamp.Core.CrossCuttingConcerns.Caching.InMemory
{
    public class MemoryCacheService(IMemoryCache cache) : CacheService
    {
        public override void AddCache<T>(string key, T value, TimeSpan? expireTime = null)
        {
            cache.Set<string>(key, JsonSerializer.Serialize<T>(value), expireTime ?? TimeSpan.FromMinutes(30));
        }

        public override T GetCache<T>(string key)
        {
            var data = cache.Get(key)!.ToString();

            return JsonSerializer.Deserialize<T>(data!)!;
        }

        public override void RemoveCache(string key)
        {
            cache.Remove(key);
        }

        public override bool IsExistCache<T>(string key, out T data)
        {
            string stringDataInCache;
            bool isExist = cache.TryGetValue(key, out stringDataInCache);
            if (!isExist)
            {
                data = default;
                return isExist;
            }

            data = JsonSerializer.Deserialize<T>(stringDataInCache!)!;
            return isExist;
        }
    }
}
