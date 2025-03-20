using System;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

namespace Feverfew.DiLib
{
    internal sealed partial class DiContext : IDisposable
    {
        private readonly DiContext _parent;

        private readonly string _name;

        private bool _isDisposed = false;

        private Stack<IDisposable> _disposableStack = new Stack<IDisposable>();

        // To maintain one and only root context, prevent to create instance.
        internal DiContext(string name) : this(null, name)
        {
        }

        private DiContext(DiContext parent, string name)
        {
            _name = name;

            _parent = parent;
            _parent?._disposableStack.Push(this);

            DiContextAudit.AuditCount(this);
        }

        internal DiContext Child<Key>(Key key)
        {
            return ChildAccessor<Key>.GetChild(this, key);
        }

        internal void Set<Type>(Type instance)
        {
            _disposableStack.Push(DependencyAccessor<Type>.Add(this, instance));
        }

        internal void Set<Type>(Func<Type> factory)
        {
            _disposableStack.Push(DependencyAccessor<Type>.Add(this, factory));
        }

        internal Type Get<Type>()
        {
            var context = this;
            var instance = default(Type);
            while (DependencyAccessor<Type>.TryGet(context, out instance) == false)
            {
                context = context._parent;
                if (context == null)
                    break;
            }

            return instance;
        }

        public override string ToString()
        {
            return _name;
        }

        internal void DisposeInternal()
        {
            if (_isDisposed == true)
                return;

            _isDisposed = true;

            foreach (var item in _disposableStack)
            {
                try
                {
                    item?.Dispose();
                }
                catch (Exception e)
                {
                    DiContextAudit.LogException(e);
                }
            }
            _disposableStack.Clear();

            DiContextAudit.AuditDecount(this);
        }
    }

    internal partial class DiContext : IDependencyContainer
    {
        IDependencyContainer IDependencyContainer.Child<Key>(Key key)
        {
            return Child(key);
        }

        void IDependencyContainer.Set<Type>(Type instance)
        {
            Set(instance);
        }

        void IDependencyContainer.Set<Type>(Func<Type> factory)
        {
            Set(factory);
        }

        Type IDependencyContainer.Get<Type>()
        {
            return Get<Type>();
        }
    }

    internal partial class DiContext : IDisposableContainer
    {
        void IDisposableContainer.AddDisposable(IDisposable disposable)
        {
            _disposableStack.Push(disposable);
        }
    }

    internal partial class DiContext : IDisposable
    {
        void IDisposable.Dispose()
        {
            DisposeInternal();
        }
    }

    internal class DiContextAudit
    {
        #region Diagnostic
        public static bool Logging = true;

        private static HashSet<DiContext> _diagnosticContexts = new();

        [Conditional("UNITY_EDITOR")]
        public static void AuditCount(DiContext context)
        {
            _diagnosticContexts.Add(context);
            if(Logging)
            {
                var logger = Containers.ProjectContext?.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
                logger.Log($"Number of DiContext: {_diagnosticContexts.Count}, Added {context}");
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void AuditDecount(DiContext context)
        {
            if (_diagnosticContexts.Contains(context))
            {
                _diagnosticContexts.Remove(context);
                if (Logging)
                {
                    var logger = Containers.ProjectContext.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
                    logger.Log($"Number of DiContext: {_diagnosticContexts.Count}, Removed {context}");
                }
            }
            else
            {
                UnityEngine.Debug.Assert(false);
            }
        }

        public static void LogException(Exception message)
        {
            var logger = Containers.ProjectContext.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
            logger.LogException(message);
        }
        #endregion
    }
}
