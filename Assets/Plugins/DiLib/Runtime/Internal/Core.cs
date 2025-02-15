using System;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

namespace Feverfew.DiLib
{
    internal partial class DiContext : IDisposable
    {
        private readonly DiContext _parent;

        private readonly List<DiContext> _childs = new();

        private List<IDisposable> _removers = new List<IDisposable>(32);

        private readonly string _name;

        // To maintain one and only root context, prevent to create instance.
        internal DiContext(string name) : this(null, name)
        {
        }

        private DiContext(DiContext parent, string name)
        {
            _name = name;

            _parent = parent;
            _parent?._childs.Add(this);

            DiContextAudit.AuditCount(this);
        }

        internal DiContext GetNewChild()
        {
            return new DiContext(this, string.Empty);
        }

        IDedendencyContainer IDedendencyContainer.GetNewChild()
        {
            return GetNewChild();
        }

        internal DiContext GetNewChild(string name)
        {
            return new DiContext(this, name);
        }

        internal void Set<Type>(Type instance)
        {
            Set(DefaultContract.Default, instance);
        }

        internal void Set<Type>(Func<Type> factory)
        {
            Set(DefaultContract.Default, factory);
        }

        internal void Set<Contract, Type>(Contract contract, Type instance)
        {
            _removers.Add(DependencyAccessor<Contract, Type>.Add((this, contract), instance));
        }

        internal void Set<Contract, Type>(Contract contract, Func<Type> factory)
        {
            _removers.Add(DependencyAccessor<Contract, Type>.Add((this, contract), factory));
        }

        internal Type Get<Type>()
        {
            return Get<DefaultContract, Type>(DefaultContract.Default);
        }

        internal Type Get<Contract, Type>(Contract contract)
        {
            var context = this;
            var instance = default(Type);
            while (DependencyAccessor<Contract, Type>.TryGet((context, contract), out instance) == false)
            {
                context = context._parent;
                if (context == null)
                    return instance;
            }

            return instance;
        }

        public override string ToString()
        {
            return _name;
        }

        internal void DisposeInternal()
        {
            foreach (var child in _childs)
                child.DisposeInternal();
            _childs.Clear();

            foreach (var remover in _removers)
                remover.Dispose();

            _removers.Clear();

            DiContextAudit.AuditDecount(this);
        }

        private class DefaultContract
        {
            public static readonly DefaultContract Default = new();

            public override string ToString()
            {
                return "DefaultContract";
            }
        }
    }

    internal partial class DiContext : IDedendencyContainer
    {
        void IDedendencyContainer.Set<Type>(Type instance)
        {
            Set(instance);
        }

        void IDedendencyContainer.Set<Type>(Func<Type> factory)
        {
            Set(factory);
        }

        void IDedendencyContainer.Set<Contract, Type>(Contract contract, Type instance)
        {
            Set(contract, instance);
        }

        void IDedendencyContainer.Set<Contract, Type>(Contract contract, Func<Type> factory)
        {
            Set(contract, factory);
        }

        Type IDedendencyContainer.Get<Type>()
        {
            return Get<Type>();
        }

        Type IDedendencyContainer.Get<Contract, Type>(Contract contract)
        {
            return Get<Contract, Type>(contract);
        }

        void IDisposable.Dispose()
        {
            DisposeInternal();
        }
    }

    internal class DiContextAudit
    {
        #region Diagnostic
        public static bool Dianostic = true;

        private static HashSet<DiContext> _diagnosticContexts = new();

        [Conditional("UNITY_EDITOR")]
        public static void AuditCount(DiContext context)
        {
            _diagnosticContexts.Add(context);
            var logger = DefaultContexts.ProjectContextDependencyContainer?.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
            logger.Log($"Number of DiContext: {_diagnosticContexts.Count}, Added {context}");
        }

        [Conditional("UNITY_EDITOR")]
        public static void AuditDecount(DiContext context)
        {
            if (_diagnosticContexts.Contains(context))
            {
                _diagnosticContexts.Remove(context);
                var logger = DefaultContexts.ProjectContextDependencyContainer.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
                logger.Log($"Number of DiContext: {_diagnosticContexts.Count}, Removed {context}");
            }
            else
            {
                UnityEngine.Debug.Assert(false);
            }
        }
        #endregion
    }
}
