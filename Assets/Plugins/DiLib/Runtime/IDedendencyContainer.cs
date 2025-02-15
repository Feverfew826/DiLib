using System;

namespace Feverfew.DiLib
{
    public interface IDedendencyContainer : IDisposable
    {
        IDedendencyContainer GetNewChild();

        void Set<Type>(Type instance);
        void Set<Type>(Func<Type> factory);
        void Set<Contract, Type>(Contract contract, Type instance);
        void Set<Contract, Type>(Contract contract, Func<Type> factory);

        Type Get<Type>();
        Type Get<Contract, Type>(Contract contract);
    }
}
