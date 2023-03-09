
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace App.Services.Auth
{
    /// <summary>
    /// 泛型类型为用户的entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuthService<T>
    {
        public IDistributedCache<object,string> _cache { get; set; }
        public AuthService() { 
        }
        public  string CreateToken(T obj)
        {
            var token = Guid.NewGuid().ToString().Replace("-","");
            _cache.Set(token, obj,new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1800),
            });
            return token;
        }
        public T ValidateToken(string token)
        {
            return (T)_cache.Get(token);
            
        }
    }
}
