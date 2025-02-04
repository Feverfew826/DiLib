using System;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

namespace Feverfew.DiLib
{
    public class DiContext : IDisposable
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

        public DiContext GetNewChild()
        {
            return new DiContext(this, string.Empty);
        }

        internal DiContext GetNewChild(string name)
        {
            return new DiContext(this, name);
        }

        public void Set<Type>(Type instance)
        {
            Set(DefaultContract.Default, instance);
        }

        public void Set<Contract, Type>(Contract contract, Type instance)
        {
            _removers.Add(InstanceAccessor<Contract, Type>.Add((this, contract), instance));
        }

        public Type Get<Type>()
        {
            return Get<DefaultContract, Type>(DefaultContract.Default);
        }

        public Type Get<Contract, Type>(Contract contract)
        {
            var context = this;
            var instance = default(Type);
            while (InstanceAccessor<Contract, Type>.TryGet((context, contract), out instance) == false)
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

        public void Dispose()
        {
            foreach (var child in _childs)
                child.Dispose();
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

    public class DiContextAudit
    {
        #region Diagnostic
        public static bool Dianostic = true;

        private static HashSet<DiContext> _diagnosticContexts = new();

        [Conditional("UNITY_EDITOR")]
        public static void AuditCount(DiContext context)
        {
            _diagnosticContexts.Add(context);
            var logger = DefaultContexts.Project?.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
            logger.Log($"Number of DiContext: {_diagnosticContexts.Count}, Added {context}");
        }

        [Conditional("UNITY_EDITOR")]
        public static void AuditDecount(DiContext context)
        {
            if (_diagnosticContexts.Contains(context))
            {
                _diagnosticContexts.Remove(context);
                var logger = DefaultContexts.Project.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
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
