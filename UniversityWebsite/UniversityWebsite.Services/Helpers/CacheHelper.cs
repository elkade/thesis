using System;
using System.Runtime.Caching;

namespace UniversityWebsite.Services.Helpers
{
    public class CacheHelper
    {
        static object _o = new object();
        public static void Add(string key, object value, TimeSpan lifeTime)
        {
            lock (_o)
                MemoryCache.Default.Add(key, value, new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + lifeTime });
        }

        public static T Get<T>(string key)
            where T : class
        {
            lock (_o)
                return (T)MemoryCache.Default.Get(key);
        }

        public static T GetOrInvoke<T>(string key, Func<object> function, TimeSpan lifeTime)
            where T : class
        {
            lock (_o)
            {
                if (MemoryCache.Default.Contains(key))
                    return Get<T>(key);
                object value = function();
                Add(key, value, lifeTime);
                return (T)value;
            }
        }

        public static void Remove(string key)
        {
            lock (_o)
                MemoryCache.Default.Remove(key);
        }

        //private static TimeSpan GetTimeSpan(LifeTime lifeTime)
        //{
        //    switch (lifeTime)
        //    {
        //        case LifeTime.Hour:
        //            return TimeSpan.FromHours(1);
        //        case LifeTime.Day:
        //            return TimeSpan.FromDays(1);
        //        case LifeTime.Week:
        //            return TimeSpan.FromDays(7);
        //        case LifeTime.Month:
        //            return TimeSpan.FromDays(30);
        //        case LifeTime.Year:
        //            return TimeSpan.FromDays(365);
        //    }
        //}

        //public enum LifeTime
        //{
        //    Hour = 0,
        //    Day = 1,
        //    Week = 2,
        //    Month = 3,
        //    Year = 4
        //}
    }
}
