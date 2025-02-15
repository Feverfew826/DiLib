using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace Feverfew.DiLib
{
    internal static class DependencyAccessor<Type>
    {
        private static Dictionary<DiContext, Func<Type>> _dictionary = new Dictionary<DiContext, Func<Type>>();

        public static IDisposable Add(DiContext context, Func<Type> factory)
        {
            Assert.IsFalse(_dictionary.ContainsKey(context), $"{{{context}, {typeof(Type)}}} is already exist.");

            _dictionary.Add(context, factory);

            return new Remover(context);
        }

        public static IDisposable Add(DiContext context, Type instance)
        {
            return Add(context, () => instance);
        }

        public static bool TryGet(DiContext context, out Type instance)
        {
            var hasPreparedDependency = _dictionary.TryGetValue(context, out var factory);

            if (hasPreparedDependency)
            {
                Assert.IsNotNull(factory, $"{{{context}, {typeof(Type)}}} No dependency is prepared.");

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
            private readonly DiContext _key;

            public Remover(DiContext key)
            {
                _key = key;
            }

            public void Dispose()
            {
                _dictionary.Remove(_key);
            }
        }
    }

    internal static class ChildAccessor<Key>
    {
        private static Dictionary<(DiContext, Key), DiContext> _dictionary = new Dictionary<(DiContext, Key), DiContext>();

        public static DiContext GetChild(DiContext context, Key key)
        {
            var childKey = (context, key);
            if (_dictionary.TryGetValue(childKey, out var child))
            {
                return child;
            }
            else
            {
                var newChild = new DiContext($"{context}/{key.ToString()}");
                _dictionary.Add(childKey, newChild);
                new Remover(childKey).AddTo(context);
                return newChild;
            }
        }

        private class Remover : IDisposable
        {
            private readonly (DiContext, Key) _childKey;

            public Remover((DiContext, Key) childKey)
            {
                _childKey = childKey;
            }

            public void Dispose()
            {
                if (_dictionary.TryGetValue(_childKey, out var child))
                {
                    child.DisposeInternal();
                    _dictionary.Remove(_childKey);
                }
            }
        }
    }
}