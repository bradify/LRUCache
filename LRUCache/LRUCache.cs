﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LRUCache
{
    /// <summary>
    /// Generic Least-recently-used cache managed by a capacity and time-to-live (TTL) of 
    /// cached items. Any value or reference type can be cached as long as it has a unique
    /// integer key. Values are loaded into the cache as needed by calling GetValue(key)
    /// by retrieving them from the database. LRUCache<T> is not thread safe.
    /// </summary>
    /// <typeparam name="T">The type of object to be cached. Can be a value or reference type.</typeparam>
    public class LRUCache<T>
    {
        private TimeSpan _ttl;
        private int _capacity;
        private IDb<T> _dao;
        private Dictionary<int, CacheItem<T>> _map;
        private LinkedList<CacheItem<T>> _linkedList;

        /// <summary>
        /// Initialize a new LRUCache cached object and associates a time-to-live (TTL)
        /// value with object in the cache. 
        /// </summary>
        /// <param name="dao">Generic data access object used to retrive items for the cache.</param>
        /// <param name="ttl">Time-to-live for individual cached items. After expiration, items are removed.</param>
        /// <param name="capacity">Maximum number of items. Defaults to 1000 items.</param>
        public LRUCache(IDb<T> dao, TimeSpan ttl, int capacity = 1000)
        {
            if (ttl == null || ttl.TotalMilliseconds < 1)
                throw new ArgumentException("Item lifetime must be greater than zero milliseconds.");

            _ttl = ttl;
            _capacity = capacity;
            _dao = dao;
            _map = new Dictionary<int, CacheItem<T>>();
            _linkedList = new LinkedList<CacheItem<T>>();
        }

        /// <summary>
        /// Gets an object from the cache. If an item exists in the cache, it is returned.
        /// If the item is not in the cache, it is retrieved from the database and added to 
        /// the cache. Once the cache reaches capacity, stale items that are beyond the TTL
        /// are removed until the cache is at capacity.
        /// </summary>
        /// <param name="key">The key of the generic items to be retrieved and cached.</param>
        /// <returns>Returns the item from cache or db. Returns null if not found.</returns>
        public T GetValue(int key)
        {
            CacheItem<T> item;

            // Search the hash, O(1) time
            if (_map.TryGetValue(key, out item))
            {
                // Item is in the cache, so update date and move to tail in O(1) time
                item.CreateDate = DateTime.Now;
                _linkedList.Remove(item);
            }
            else
            {
                // Item is not in cache, so retrieve from db and move to the tail
                var obj = _dao.GetFromDatabase(key);
                if (obj != null)
                {
                    item = new CacheItem<T> { Key = key, CachedObject = obj, CreateDate = DateTime.Now };
                    _map.Add(key, item);
                }
                else
                {
                    return default(T);
                }
            }

            // Add item linked list, O(1) time
            _linkedList.AddLast(item);
            CleanUp();

            return item.CachedObject;
        }

        /// <summary>
        /// Enumerates the cache
        /// </summary>
        public IEnumerable<CacheItem<T>> GetAllValues()
        {
            return _linkedList.ToList();
        }

        /// <summary>
        /// Starts at the head pointer and walks the linked list looking to remove
        /// nodes that have lived longer than the TimeSpan passed into the constructor.
        /// When an expired item is found, the node is removed from the linked list in O(1) time
        /// and the index is removed from the hash. Since the linked list is always sorted by 
        /// date in ascending order, CleanUp() finishes when a node with a valid lifetime is found.
        /// </summary>
        private void CleanUp()
        {
            while (_linkedList.First != null)
            {
                TimeSpan timeInExistence = DateTime.Now.Subtract(_linkedList.First.Value.CreateDate);
                if (timeInExistence > _ttl || _linkedList.Count > _capacity)
                {
                    // Removal is an O(1) operation
                    _map.Remove(_linkedList.First.Value.Key);
                    _linkedList.RemoveFirst();
                }
                else
                {
                    return;
                }
            }
        }
    }
}