using UnityEngine;

namespace Feverfew.DiLib.Samples.UsageSample
{
    [DefaultExecutionOrder(-1)]
    public class SetUsage : MonoBehaviour
    {
        private void Awake()
        {
            DiLib.DefaultContexts.Project.Set("Hello, world!");

            var sceneContext = DiLib.DefaultContexts.GetSceneContext(gameObject.scene);

            sceneContext.Set(16);
            sceneContext.Set(new TestParameter());
        }
    }
}
