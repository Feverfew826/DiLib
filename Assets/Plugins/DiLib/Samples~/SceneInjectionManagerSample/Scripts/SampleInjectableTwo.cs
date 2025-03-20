using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feverfew.DiLib.Samples.SceneInjectionManagerSample
{
    public class SampleInjectableTwo : MonoBehaviour, IInjectable
    {
        private string _message;

        void IInjectable.Inject(IDependencyContainer container)
        {
            _message = container.Get<string>();
        }

        private void Start()
        {
            Debug.Log(_message, this);
        }
    }
}
