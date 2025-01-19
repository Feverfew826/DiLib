using System;
using System.Collections.Generic;

namespace Feverfew.DiLib
{
    internal static class InstanceAccessor<Contract, Type>
    {
        private static Dictionary<(DiContext, Contract), Type> _dictionary = new Dictionary<(DiContext, Contract), Type>();

        public static IDisposable Add((DiContext, Contract) key, Type instance)
        {
            _dictionary.Add(key, instance);

            return new Remover(key);
        }

        public static bool TryGet((DiContext, Contract) key, out Type instance)
        {
            return _dictionary.TryGetValue(key, out instance);
        }

        private class Remover : IDisposable
        {
            private readonly (DiContext, Contract) _key;

            public Remover((DiContext, Contract) key)
            {
                _key = key;
            }

            public void Dispose()
            {
                _dictionary.Remove(_key);
            }
        }
    }
}