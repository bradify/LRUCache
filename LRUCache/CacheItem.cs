using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    /// <summary>
    /// Individual item in the cache. Item will require a unique ID.
    /// </summary>
    /// <typeparam name="T">Generic type of the cached item.</typeparam>
    public class CacheItem<T>
    {
        /// <summary>
        /// Unique ID of the cached item
        /// </summary>
        public int Key;

        /// <summary>
        /// The cached item
        /// </summary>
        public T CachedObject;

        /// <summary>
        /// The date the item was added/updated in the cache
        /// </summary>
        public DateTime CreateDate;

        /// <summary>
        /// Initializes a new CacheItem with the current date/time
        /// </summary>
        public CacheItem()
        {
            CreateDate = DateTime.Now;
        }
    }
}
