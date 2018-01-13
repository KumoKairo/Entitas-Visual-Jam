using System;
using System.Collections.Generic;

namespace PureMVC.Utils
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, 
            Func<TKey, TValue> initializeRoutine)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                TValue value = initializeRoutine(key);
                dictionary[key] = value;
                return value;
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                dictionary[key] = value;
                return value;
            }
        }

        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }
            else
            {
                dictionary[key] = value;
                return true;
            }
        }

        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, out TValue value)
        {
            value = default(TValue);

            if (dictionary.ContainsKey(key))
            {
                value = dictionary[key];
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}