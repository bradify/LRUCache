using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    /// <summary>
    /// Interface that supports CRUD operations
    /// </summary>
    /// <typeparam name="TValue">The generic type</typeparam>
    public interface IDb<TValue>
    {
        /// <summary>
        /// Get a generic item from the database
        /// </summary>
        /// <param name="key">The unique key of the item</param>
        /// <returns>The item retrieved. Type is defined by user on implementation or instantiation of IDb</returns>
        TValue GetFromDatabase(int key);
    }
}
