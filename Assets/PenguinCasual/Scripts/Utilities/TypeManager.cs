using System;
using System.Collections.Generic;

namespace Penguin
{
    public static class TypeManager<T>
    {
        private static Dictionary<Type, int> _typeToIndex = new Dictionary<Type, int>();
        private static Type[] _types;

        public static int TotalType => _types?.Length ?? 0;

        public static int RegisterType(Type type)
        {
            if (_typeToIndex.ContainsKey(type))
                throw new Exception("Sum ting wong");

            int index = _typeToIndex.Count;
            _typeToIndex[type] = index;
            return index;
        }

        public static int IndexOf(Type type)
        {
            if (_typeToIndex.TryGetValue(type, out var index))
            {
                return index;
            }

            return -1;
        }

        public static void CollectTypes()
        {
            _types = TypeCollector.GetStructsOfInterface(typeof(T));
        }

        public static void Clear()
        {
            _typeToIndex.Clear();
        }
    }
}