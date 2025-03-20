using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feverfew.DiLib
{
    public interface IInjectable
    {
        void Inject(IDependencyContainer container);
    }
}
