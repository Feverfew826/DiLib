using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace Feverfew.DiLib
{
    internal static class InstanceAccessor<Contract, Type>
    {
        private static Dictionary<(DiContext, Contract), Type> _dictionary = new Dictionary<(DiContext, Contract), Type>();

        public static IDisposable Add((DiContext context, Contract contract) key, Type instance)
        {
            Assert.IsFalse(_dictionary.ContainsKey(key), $"Duplicated '{typeof(Type)}' type instance are prepared on same DI context({key.context}) and same contract({key.contract}).");

            _dictionary.Add(key, instance);

            return new Remover(key);
        }

        public static bool TryGet((DiContext, Contract) key, out Type instance)
        {
            var hasInstance = _dictionary.TryGetValue(key, out instance);

            return hasInstance;
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