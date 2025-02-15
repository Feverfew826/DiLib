using UnityEngine;

namespace Feverfew.DiLib.Samples.UsageSample
{
    [DefaultExecutionOrder(-1)]
    public class SetUsage : MonoBehaviour
    {
        private long _increaseEachTimeGetLong = 64L;

        private void Awake()
        {
            var sceneContext = DiLib.Containers.SceneContext(gameObject.scene);

            sceneContext.Set(32f);
            sceneContext.Set(new TestParameter().AddTo(sceneContext));
            sceneContext.Set(() => _increaseEachTimeGetLong++);

            sceneContext.Set("SceneContext");
            sceneContext.Child("Child1").Set("SceneContext/Child1");
            sceneContext.Child("Child2").Set("SceneContext/Child2");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InjectProjectDependency()
        {
            DiLib.Containers.ProjectContext.Set("Hello, world!");
        }
    }
}
