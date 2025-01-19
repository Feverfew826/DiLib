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
        internal DiContext(string name)
        {
            _diagnosticContextName = name;

            Count(_diagnosticContextName);
        }

        private DiContext(DiContext parent, string name) : this(name)
        {
            _parent = parent;
            _parent._childs.Add(this);
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

            Decount(_diagnosticContextName);
        }

        private class DefaultContract
        {
            public static DefaultContract Default = new();
        }

        #region Diagnostic
        public static bool Dianostic = true;

        private static List<string> _diagnosticContextNames = new();

        private string _diagnosticContextName;

        [Conditional("UNITY_EDITOR")]
        private void Count(string name)
        {
            _diagnosticContextNames.Add(name);
            var logger = DefaultContexts.Project?.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
            logger.Log($"Number of DiContext: {_diagnosticContextNames.Count}");
        }

        [Conditional("UNITY_EDITOR")]
        private void Decount(string name)
        {
            _diagnosticContextNames.Add(name);
            var logger = DefaultContexts.Project.Get<ILogger>() ?? UnityEngine.Debug.unityLogger;
            logger.Log($"Number of DiContext: {_diagnosticContextNames.Count}");
        }
        #endregion
    }
}
