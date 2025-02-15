using UnityEngine;

namespace Feverfew.DiLib.Samples.UsageSample
{
    public class GetUsage : MonoBehaviour
    {
        private void Awake()
        {
            var message = DiLib.Containers.ProjectContext.Get<string>();
            Debug.Log(message);

            var sceneContext = DiLib.Containers.SceneContext(gameObject.scene);

            var floatValue = sceneContext.Get<float>();
            Debug.Log(floatValue);

            var testParameter = sceneContext.Get<TestParameter>();
            Debug.Log(testParameter.Id);

            var increaseEachTimeGetLong = sceneContext.Get<long>();
            Debug.Log(increaseEachTimeGetLong);
            increaseEachTimeGetLong = sceneContext.Get<long>();
            Debug.Log(increaseEachTimeGetLong);
            increaseEachTimeGetLong = sceneContext.Get<long>();
            Debug.Log(increaseEachTimeGetLong);

            var sceneContextString = sceneContext.Get<string>();
            Debug.Log(sceneContextString);
            var child1ContextString = sceneContext.Child("Child1").Get<string>();
            Debug.Log(child1ContextString);
            var child2ContextString = sceneContext.Child("Child2").Get<string>();
            Debug.Log(child2ContextString);
        }
    }
}
