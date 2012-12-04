using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class CustomJsonConfigDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ISerializable
        where TValue : class
        where TKey : class
    {
        private readonly Dictionary<TKey, TValue> _internalDictionary = new Dictionary<TKey, TValue>();

        protected CustomJsonConfigDictionary(SerializationInfo info, StreamingContext context)
        {
            foreach (var entry in info)
            {
                var value = entry.Value as TValue;
                var name = entry.Name as TKey;
                _internalDictionary.Add(name, value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (TKey key in _internalDictionary.Keys)
            {
                var value = _internalDictionary[key];
                info.AddValue(key.ToString(), value);
            }
        }

        public TValue this[TKey key]
        {
            get { return _internalDictionary[key]; }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}