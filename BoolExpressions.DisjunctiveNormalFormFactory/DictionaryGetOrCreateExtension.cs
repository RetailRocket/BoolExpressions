namespace BoolExpressions.DisjunctiveNormalFormFactory
{
    using System;
    using System.Collections.Generic;

    internal static class DictionaryGetOrCreateExtension
    {
        public static TValue GetOrCreate<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> creator)
            where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            value = creator();
            dictionary.Add(key, value);

            return value;
        }
    }
}