using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feverfew.DiLib.Samples.SceneInjectionManagerSample
{
    public class SampleDependencyPreparer : MonoBehaviour
    {
        public void Prepare(IDependencyContainer container)
        {
            Debug.Log("If writing a preparer is too bothersome, you don’t have to write one. Just set the dependencies as shown in the UsageSample scene. Make sure to set the dependencies before calling get.");
            container.Set<int>(42);
            container.Set<string>("Injected message");
        }
    }
}
