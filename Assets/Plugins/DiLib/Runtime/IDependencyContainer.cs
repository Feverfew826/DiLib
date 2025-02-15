using System;

namespace Feverfew.DiLib
{
    public interface IDependencyContainer : IDisposable, IDisposableContainer
    {
        IDependencyContainer Child<Key>(Key key);

        void Set<Type>(Type instance);
        void Set<Type>(Func<Type> factory);

        Type Get<Type>();
    }

    public interface IDisposableContainer : IDisposable
    {
        void AddDisposable(IDisposable disposable);
    }

    public static class Utility
    {
        public static T AddTo<T>(this T disposable, IDependencyContainer dedendencyContainer) where T : IDisposable
        {
            dedendencyContainer.AddDisposable(disposable);

            return disposable;
        }
    }
}
