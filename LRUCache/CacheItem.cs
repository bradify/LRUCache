using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    public class CacheItem<T>
    {
        public int Key;
        public T CachedObject;
        public DateTime CreateDate;

        public CacheItem()
        {
            CreateDate = DateTime.Now;
        }
    }
}
