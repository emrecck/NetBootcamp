using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetBootcamp.Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisService : CacheService
    {
        private readonly IDatabase Database;
        public RedisService(string url)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(url);
            connectionMultiplexer.ConnectionFailed += ConnectionMultiplexer_ConnectionFailed;
            Database = connectionMultiplexer.GetDatabase();
        }

        public override void AddCache<T>(string key, T value, TimeSpan? expireTime = null)
        {
            Database.StringSet(key, JsonSerializer.Serialize<T>(value), expireTime);
        }

        public override T GetCache<T>(string key)
        {
            var data = Database.StringGet(key);
            return JsonSerializer.Deserialize<T>(data!)!;
        }

        public override bool IsExistCache<T>(string key, out T data)
        {
            var isExist = Database.KeyExists(key);
            if (!isExist)
            {
                data = default!;
                return isExist;
            }

            data = JsonSerializer.Deserialize<T>(Database.StringGet(key)!)!;
            return isExist;
        }

        public override void RemoveCache(string key)
        {
            Database.KeyDelete(key);
        }

        private void ConnectionMultiplexer_ConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
