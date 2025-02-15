using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace Feverfew.DiLib
{
    internal static class DependencyAccessor<Contract, Type>
    {
        private static Dictionary<(DiContext, Contract), Func<Type>> _dictionary = new Dictionary<(DiContext, Contract), Func<Type>>();

        public static IDisposable Add((DiContext context, Contract contract) key, Func<Type> factory)
        {
            Assert.IsFalse(_dictionary.ContainsKey(key), $"Duplicated '{typeof(Type)}' type instance are prepared on same DI context({key.context}) and same contract({key.contract}).");

            _dictionary.Add(key, factory);

            return new Remover(key);
        }

        public static IDisposable Add((DiContext context, Contract contract) key, Type instance)
        {
            return Add(key, () => instance);
        }

        public static bool TryGet((DiContext context, Contract contract) key, out Type instance)
        {
            var hasPreparedDependency = _dictionary.TryGetValue(key, out var factory);

            if (hasPreparedDependency)
            {
                Assert.IsNotNull(factory, $"Factory of type '{typeof(Type)} on context({key.context}) of contract({key.contract})' are null.");

                instance = factory.Invoke() ?? default;
            }
            else
            {
                instance = default;
            }

            return hasPreparedDependency;
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