using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    public interface IDb<TValue>
    {
        TValue GetFromDatabase(int key);
    }
}
