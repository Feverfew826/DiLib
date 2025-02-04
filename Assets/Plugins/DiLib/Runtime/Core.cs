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

        // To maintain one and only root context, prevent to create instance.
        internal DiContext(string name) : this(null, name)
        {
        }

        private DiContext(DiContext parent, string name)
        {
            _parent = parent;
            _parent?._childs.Add(this);

            DiContextAudit.AuditCount(this, name);
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
            public static DefaultContract Default = new();
        }
    }

    public class DiContextAudit
    {
        #region Diagnostic
        public static bool Dianostic = true;

        private static Dictionary<DiContext, string> _diagnosticContextNames = new();

        [Conditional("UNITY_EDITOR")]
        public static void AuditCount(DiContext context, string name)
        {
            _diagnosticContextNames.Add(context, name);
            var logger = DefaultContexts.Project?.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
            logger.Log($"Number of DiContext: {_diagnosticContextNames.Count}, Added {name}");
        }

        [Conditional("UNITY_EDITOR")]
        public static void AuditDecount(DiContext context)
        {
            if (_diagnosticContextNames.TryGetValue(context, out var name))
            {
                _diagnosticContextNames.Remove(context);
                var logger = DefaultContexts.Project.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
                logger.Log($"Number of DiContext: {_diagnosticContextNames.Count}, Removed {name}");
            }
            else
            {
                UnityEngine.Debug.Assert(false);
            }
        }
        #endregion
    }
}
