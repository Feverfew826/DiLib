using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feverfew.DiLib.Samples.SceneInjectionManagerSample
{
    public class SampleInjectableOne : MonoBehaviour, IInjectable
    {
        private int _number;
        public void Inject(IDependencyContainer container)
        {
            _number = container.Get<int>();
        }

        private void Awake()
        {
            Debug.Log(_number, this);
        }
    }
}
