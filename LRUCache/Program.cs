using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LRUCache
{
    /// <summary>
    /// A quick smoke test of the LRUCache implementation
    /// </summary>
    class Program
    {
        public class User
        {
            public int ID;
            public string Username;
            public string Password;
            public string FavoriteFood;
        }

        public class MockUserRepo : IDb<User>
        {
            public User GetFromDatabase(int key)
            {
                switch (key)
                {
                    case 0:
                        return new User
                        {
                            ID = 0,
                            Username = "Brad",
                            Password = "123",
                            FavoriteFood = "tacos"
                        };
                    case 1:
                        return new User
                        {
                            ID = 1,
                            Username = "Brendan",
                            Password = "123",
                            FavoriteFood = "curry"
                        };
                    case 2:
                        return new User
                        {
                            ID = 2,
                            Username = "Sim",
                            Password = "123",
                            FavoriteFood = "unknown"
                        };
                    default:
                        return new User
                        {
                            ID = 999,
                            Username = "Robot",
                            Password = "123",
                            FavoriteFood = "WD-40"
                        };
                }
            }
        }

        static void Main(string[] args)
        {
            // New User cache with a 2 second TTL and a capacity of 3 items.
            var cache = new LRUCache<User>(new MockUserRepo(), new TimeSpan(0, 0, 2), 3);

            cache.GetValue(0);
            cache.GetValue(1);
            Thread.Sleep(3001); // milliseconds
            cache.GetValue(2);

            Console.WriteLine("\nLRU Cache Contents 1");
            PrintValues(cache);

            cache.GetValue(0);
            cache.GetValue(3);
            cache.GetValue(0);
            cache.GetValue(0);
            cache.GetValue(0);
            cache.GetValue(0);
            cache.GetValue(0);
            cache.GetValue(0);
            cache.GetValue(3);

            Console.WriteLine("\nLRU Cache Contents 2");
            PrintValues(cache);

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }

        private static void PrintValues(LRUCache<User> cache)
        {
            foreach(var cachedItem in cache.GetAllValues())
            {
                Console.WriteLine(string.Format("ID: {0}\t{1} ({2})",
                    cachedItem.CachedObject.ID,
                    cachedItem.CachedObject.Username,
                    cachedItem.CachedObject.FavoriteFood));
            }
        }
    }
}
